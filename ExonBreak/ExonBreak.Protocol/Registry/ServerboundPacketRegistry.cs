using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Server.Protocol;

namespace ExonBreak.Protocol.Registry;

public class ServerboundPacketRegistry : PacketRegistry
{
    public ServerboundPacketRegistry(Action<string> logFunction, ProtocolSide side) : base(logFunction, side)
    {
        Register(ServerboundHandshakeRequestPacket.SERIALIZER);
        Register(ServerboundAttemptLoginPacket.SERIALIZER);
    }
}
