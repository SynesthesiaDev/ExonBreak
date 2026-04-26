using ExonBreak.Protocol.Packets.Handshake;

namespace ExonBreak.Protocol.Registry;

public class ServerboundPacketRegistry : PacketRegistry
{
    public ServerboundPacketRegistry()
    {
        Register(ServerboundHandshakeRequestPacket.SERIALIZER);
        Register(ServerboundAttemptLoginPacket.SERIALIZER);
    }
}
