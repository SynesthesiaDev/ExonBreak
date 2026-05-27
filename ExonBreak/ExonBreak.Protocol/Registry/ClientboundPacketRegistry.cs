using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Packets.Login;
using ExonBreak.Server.Protocol;

namespace ExonBreak.Protocol.Registry;

public class ClientboundPacketRegistry : PacketRegistry
{
    public ClientboundPacketRegistry(Action<string> logFunction, ProtocolSide side): base(logFunction, side)
    {
        Register(ClientboundHandshakeResponsePacket.SERIALIZER);
        Register(ClientboundDisconnectPacket.SERIALIZER);
        Register(ClientboundAcceptLoginPacket.SERIALIZER);
        Register(ClientboundEncryptionRequestPacket.SERIALIZER);
        Register(ClientboundEncryptionSuccessPacket.SERIALIZER);
    }
}
