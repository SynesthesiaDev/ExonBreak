using System;
using DotNetty.Transport.Channels;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Types;
using ExonBreak.Server.Protocol;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class ClientPacketHandler(GameClient client) : PacketHandler
{
    private PacketContext packetContext = null!;
    public readonly GameClient GameClient = client;

    public override void ChannelActive(IChannelHandlerContext context)
    {
        packetContext = new PacketContext(context, log => Logger.Log(log, LoggingTarget.Network), ProtocolSide.Client);
        base.ChannelActive(context);

    }

    public override void OnConnected(IChannel channel)
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
