using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Server.Protocol;

namespace ExonBreak.Protocol.Registry;

public class ClientboundPacketRegistry : PacketRegistry
{
    public ClientboundPacketRegistry(Action<string> logFunction, ProtocolSide side): base(logFunction, side)
    {
        Register(ClientboundHandshakeResponsePacket.SERIALIZER);
        Register(ClientboundDenyLoginPacket.SERIALIZER);
        Register(ClientboundAcceptLoginPacket.SERIALIZER);
    }
}
