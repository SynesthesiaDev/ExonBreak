using DotNetty.Transport.Channels;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets;
using ExonBreak.Protocol.Types.Player;
using Serilog;

namespace ExonBreak.Server.Protocol.Netty;

public class ServerPlayerConnection : PlayerConnection
{
    private PacketContext packetContext = null!;
    public override IChannelHandlerContext ChannelHandlerContext { get; set; } = null!;

    public byte[]? PendingChallengeBytes = null;

    public PlayerInfo PlayerInfo = null!;

    public override void ChannelActive(IChannelHandlerContext context)
    {
        ChannelHandlerContext = context;
        Log.Information("(Server) New client connection established: {ip}", context.Channel.RemoteAddress.ToString());
        packetContext = new PacketContext(this, Log.Verbose, ProtocolSide.Server);
        base.ChannelActive(context);
    }

    public override void ChannelInactive(IChannelHandlerContext context)
    {
        Log.Information("(Server) Client {username} ({uuid}) has disconnected", PlayerInfo.Username, PlayerInfo.Id);
        PendingChallengeBytes = null;
        base.ChannelInactive(context);
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
