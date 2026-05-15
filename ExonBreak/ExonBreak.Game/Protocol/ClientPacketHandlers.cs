using ExonBreak.Game.Protocol.Handlers;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Registry;

namespace ExonBreak.Game.Protocol;

public class ClientPacketHandlers
{
    public static void RegisterHandlers(ClientboundPacketRegistry packetRegistry)
    {
        packetRegistry.AddHandler<ClientboundHandshakeResponsePacket>(ClientHandshakeHandlers.HandleHandshake);
        packetRegistry.AddHandler<ClientboundAcceptLoginPacket>(ClientHandshakeHandlers.HandleAttemptLoginAccept);
        packetRegistry.AddHandler<ClientboundDenyLoginPacket>(ClientHandshakeHandlers.HandleAttemptLoginDeny);

        // packetRegistry.AddHandler<ServerboundHandshakeRequestPacket>(ServerHandshakeHandlers.HandleHandshake);
        // packetRegistry.AddHandler<ServerboundAttemptLoginPacket>(ServerHandshakeHandlers.HandleLoginAttempt);
    }
}
