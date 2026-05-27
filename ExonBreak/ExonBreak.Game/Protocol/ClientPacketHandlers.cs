using ExonBreak.Game.Protocol.Handlers;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Packets.Login;
using ExonBreak.Protocol.Registry;

namespace ExonBreak.Game.Protocol;

public class ClientPacketHandlers
{
    public static void RegisterHandlers(ClientboundPacketRegistry packetRegistry)
    {
        packetRegistry.AddHandler<ClientboundHandshakeResponsePacket>(ClientHandshakeHandlers.HandleHandshake);
        packetRegistry.AddHandler<ClientboundAcceptLoginPacket>(ClientHandshakeHandlers.HandleAttemptLoginAccept);
        packetRegistry.AddHandler<ClientboundDisconnectPacket>(ClientHandshakeHandlers.HandleAttemptLoginDeny);
        packetRegistry.AddHandler<ClientboundEncryptionRequestPacket>(ClientboundLoginHandlers.HandleEncryptionRequest);
        packetRegistry.AddHandler<ClientboundEncryptionSuccessPacket>(ClientboundLoginHandlers.HandleEncryptionSuccess);
    }
}
