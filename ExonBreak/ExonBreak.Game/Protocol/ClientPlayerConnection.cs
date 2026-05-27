using System;
using DotNetty.Transport.Channels;
using ExonBreak.Game.Persistent;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Server.Protocol;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class ClientPlayerConnection(GameClient client) : PlayerConnection
{
    private PacketContext packetContext = null!;
    public readonly GameClient GameClient = client;

    public readonly PlayerIdentity PlayerIdentity = ExonBreakGameBase.Identity;

    public override IChannelHandlerContext ChannelHandlerContext { get; set; } = null!;

    public override void ChannelActive(IChannelHandlerContext context)
    {
        ChannelHandlerContext = context;
        packetContext = new PacketContext(this, log => Logger.Log(log, LoggingTarget.Network), ProtocolSide.Client);
        base.ChannelActive(context);
    }

    public void OnConnected(IChannel channel)
    {
        packetContext.SendPacket(new ServerboundHandshakeRequestPacket(SharedConstants.PROTOCOL_VERSION, GameClient.PlayerInfo));
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, WrappedPacket msg)
    {
        try
        {
            GameClient.CLIENTBOUND_PACKET_REGISTRY.ProcessPacket(msg, packetContext);
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Failed to read packet", LoggingTarget.Network);
        }
        finally
        {
            msg.Data.Release();
        }
    }

}
