using System;
using DotNetty.Transport.Channels;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets;
using ExonBreak.Protocol.Packets.Handshake;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class ClientPacketHandler(GameClient client) : SimpleChannelInboundHandler<WrappedPacket>
{
    private PacketContext packetContext = null!;
    public readonly GameClient GameClient = client;

    public override void ChannelActive(IChannelHandlerContext context)
    {
        packetContext = new PacketContext(context);
        packetContext.SendPacket(new ServerboundHandshakeRequestPacket(SharedConstants.PROTOCOL_VERSION, GameClient.PlayerInfo));

        base.ChannelActive(context);
    }

    protected override void ChannelRead0(IChannelHandlerContext ctx, WrappedPacket msg)
    {
        Logger.Log($"Received packet with id: {msg.Id}", LoggingTarget.Network);
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
