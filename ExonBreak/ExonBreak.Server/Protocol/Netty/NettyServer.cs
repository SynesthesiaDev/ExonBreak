using System.Net;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using ExonBreak.Server.Config;
using Serilog;

namespace ExonBreak.Server.Protocol.Netty;

public class NettyServer
{
    public async Task StartAsync()
    {
        var bossGroup = new MultithreadEventLoopGroup(1);
        var workerGroup = new MultithreadEventLoopGroup();

        try
        {
            var bootstrap = new ServerBootstrap();
            bootstrap.Group(bossGroup, workerGroup)
                .Channel<TcpServerSocketChannel>()
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;

                    //TODO later: encryption

                    // Inbound
                    pipeline.AddLast(new InboundFrameDecoder());
                    pipeline.AddLast(new InboundWrappedPacketDecoder());
                    pipeline.AddLast(new ServerPacketHandler());

                    // Outbound
                    pipeline.AddLast(new OutboundLengthPrepender());
                    pipeline.AddLast(new OutboundWrappedPacketEncoder());
                    pipeline.AddLast(new OutboundPacketEncoder());

                    // Handling
                    pipeline.AddLast(new ServerPacketHandler());
                }));

            var endPoint = new IPEndPoint(IPAddress.Parse(ServerConfig.Ip), ServerConfig.Port);
            IChannel boundChannel = await bootstrap.BindAsync(endPoint);

            Log.Information("Server started with ip {Ip} and port {Port}", ServerConfig.Ip, ServerConfig.Port);

            await boundChannel.CloseCompletion;
        }
        finally
        {
            await Task.WhenAll(
                bossGroup.ShutdownGracefullyAsync(),
                workerGroup.ShutdownGracefullyAsync()
            );
        }
    }
}
