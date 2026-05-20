using DotNetty.Transport.Channels;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets;
using Serilog;

namespace ExonBreak.Server.Protocol.Netty;

public class ServerPacketHandler : SimpleChannelInboundHandler<WrappedPacket>
{
    private PacketContext packetContext = null!;

    public override void ChannelActive(IChannelHandlerContext context)
    {
        Log.Information("(Server) New client connection established: {ip}", context.Channel.RemoteAddress.ToString());
        packetContext = new PacketContext(context, Log.Verbose, ProtocolSide.Server);
        base.ChannelActive(context);
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, WrappedPacket msg)
    {
        try
        {
            DedicatedServer.SERVERBOUND_PACKET_REGISTRY.ProcessPacket(msg, packetContext);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to read packet with id {Id}", msg.Id);
        }
        finally
        {
            msg.Data.Release();
        }
    }
}
