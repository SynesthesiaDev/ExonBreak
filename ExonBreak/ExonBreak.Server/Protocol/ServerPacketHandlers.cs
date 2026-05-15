using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Registry;
using ExonBreak.Server.Protocol.Handlers;

namespace ExonBreak.Server.Protocol;

public class ServerPacketHandlers
{
    public static void RegisterHandlers(ServerboundPacketRegistry packetRegistry)
    {
        packetRegistry.AddHandler<ServerboundHandshakeRequestPacket>(ServerHandshakeHandlers.HandleHandshake);
        packetRegistry.AddHandler<ServerboundAttemptLoginPacket>(ServerHandshakeHandlers.HandleLoginAttempt);
    }
}
