using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Registry;
using ExonBreak.Server.Protocol.Handlers;

namespace ExonBreak.Server.Protocol;

public class PacketHandlers
{
    public static void RegisterHandlers(ServerboundPacketRegistry packetRegistry)
    {
        packetRegistry.AddHandler<ServerboundHandshakeRequestPacket>(HandshakeHandlers.HandleHandshake);
        packetRegistry.AddHandler<ServerboundAttemptLoginPacket>(HandshakeHandlers.HandleLoginAttempt);
    }
}