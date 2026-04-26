using ExonBreak.Protocol.Packets.Handshake;

namespace ExonBreak.Protocol.Registry;

public class ClientboundPacketRegistry : PacketRegistry
{
    public ClientboundPacketRegistry()
    {
        Register(ClientboundHandshakeResponsePacket.SERIALIZER, null);
        Register(ClientboundDenyLoginPacket.SERIALIZER, null);
        Register(ClientboundAcceptLoginPacket.SERIALIZER, null);
    }
}
