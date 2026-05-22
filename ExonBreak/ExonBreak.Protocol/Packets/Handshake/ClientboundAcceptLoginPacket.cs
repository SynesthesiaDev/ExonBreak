using Codon.Binary;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ClientboundAcceptLoginPacket: IPacket
{
    public static readonly IBinaryCodec<ClientboundAcceptLoginPacket> CODEC = BinaryCodecs.Empty(() => new ClientboundAcceptLoginPacket());

    public static readonly ProtocolSerializer<ClientboundAcceptLoginPacket> SERIALIZER = Serializers.FromCodec(CODEC);

}
