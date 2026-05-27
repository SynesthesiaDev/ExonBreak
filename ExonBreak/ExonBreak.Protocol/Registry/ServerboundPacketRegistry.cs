using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Packets.Login;
using ExonBreak.Server.Protocol;

namespace ExonBreak.Protocol.Registry;

public class ServerboundPacketRegistry : PacketRegistry
{
    public ServerboundPacketRegistry(Action<string> logFunction, ProtocolSide side) : base(logFunction, side)
    {
        Register(ServerboundHandshakeRequestPacket.SERIALIZER);
        Register(ServerboundAttemptLoginPacket.SERIALIZER);
        Register(ServerboundEncryptionResponsePacket.SERIALIZER);
    }
}
