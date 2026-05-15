using ExonBreak.Protocol.Packets.Handshake;

namespace ExonBreak.Protocol.Registry;

public class ClientboundPacketRegistry : PacketRegistry
{
    public ClientboundPacketRegistry()
    {
        Register(ClientboundHandshakeResponsePacket.SERIALIZER);
        Register(ClientboundDenyLoginPacket.SERIALIZER);
        Register(ClientboundAcceptLoginPacket.SERIALIZER);
    }
}
