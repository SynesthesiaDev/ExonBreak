using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using ExonBreak.Server.Protocol.Netty;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class NettyClient(string ip, int port, GameClient client) : IDisposable
{
    private IChannel channel = null!;
    private IEventLoopGroup group = null!;
    private ClientPlayerConnection playerConnection = null!;

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
                pipeline.AddLast(playerConnection = new ClientPlayerConnection(client));

                // Outbound
                pipeline.AddLast(new OutboundLengthPrepender());
                pipeline.AddLast(new OutboundWrappedPacketEncoder());
                pipeline.AddLast(new OutboundClientPacketEncoder());
            }));

        try
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Logger.Log($"Attempting connection to {ip}:{port}...", LoggingTarget.Network);

            var connectTask = bootstrap.ConnectAsync(endpoint);
            var timeoutTask = Task.Delay(5000);

            var completed = await Task.WhenAny(connectTask, timeoutTask);

            if (completed == connectTask)
            {
                channel = await connectTask;
                Logger.Log($"Connected to server at {ip}:{port}", LoggingTarget.Network);
                playerConnection.OnConnected(channel);
                _ = waitForCloseAsync();
            }
            else
            {
                Logger.Log("Failed to connect.. connection timed out", LoggingTarget.Network, LogLevel.Error);
                await waitForCloseAsync();
            }
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
            Logger.Log("Disconnected, event loop shut down", LoggingTarget.Network);
            Dispose();
        }
    }

    public void Dispose()
    {
        group.ShutdownGracefullyAsync();
        client.OnDisconnected.Dispatch(client);
    }
}
