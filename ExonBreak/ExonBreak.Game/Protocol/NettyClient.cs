using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using ExonBreak.Server.Protocol.Netty;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class NettyClient(string ip, int port, GameClient client)
{
    private IChannel channel = null!;
    private IEventLoopGroup group = null!;

    public async Task ConnectAsync()
    {
        group = new MultithreadEventLoopGroup();

        var bootstrap = new Bootstrap();
        bootstrap.Group(group)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ActionChannelInitializer<IChannel>(ch =>
            {
                var pipeline = ch.Pipeline;

                // Inbound
                pipeline.AddLast(new InboundFrameDecoder());
                pipeline.AddLast(new InboundWrappedPacketDecoder());
                pipeline.AddLast(new ClientPacketHandler(client));

                // Outbound
                pipeline.AddLast(new OutboundLengthPrepender());
                pipeline.AddLast(new OutboundWrappedPacketEncoder());
                pipeline.AddLast(new OutboundClientPacketEncoder());
            }));


        try
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Logger.Log($"Attempting connection to {ip}:{port}...", LoggingTarget.Network);

            channel = await bootstrap.ConnectAsync(endpoint);

            Logger.Log($"Connected to server at {ip}:{port}", LoggingTarget.Network);
            _ = waitForCloseAsync();
        }
        catch (Exception ex)
        {
            Logger.Log($"Connection failed: {ex.GetType().Name}: {ex.Message}", LoggingTarget.Network, LogLevel.Error);
            await group.ShutdownGracefullyAsync();
            throw;
        }
    }

    private async Task waitForCloseAsync()
    {
        try
        {
            await channel.CloseCompletion;
        }
        finally
        {
            await group.ShutdownGracefullyAsync();
            Logger.Log("Disconnected, event loop shut down", LoggingTarget.Network);
        }
    }

    public async Task SendAsync(object packet)
    {
        if (channel is { Active: true })
        {
            Logger.Log($"<- {packet.GetType().Name}", LoggingTarget.Network);
            await channel.WriteAndFlushAsync(packet);
        }
        else
            Logger.Log("Attempted to send packet but channel is not active", LoggingTarget.Network, LogLevel.Error);
    }

    public async Task DisconnectAsync()
    {
        if (channel != null) await channel.CloseAsync();
    }
}
