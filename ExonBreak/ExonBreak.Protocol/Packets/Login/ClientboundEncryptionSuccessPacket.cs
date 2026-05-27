using Codon.Binary;

namespace ExonBreak.Protocol.Packets.Login;

public record ClientboundEncryptionSuccessPacket : IPacket
{
    public static readonly IBinaryCodec<ClientboundEncryptionSuccessPacket> CODEC = BinaryCodecs.Empty(() => new ClientboundEncryptionSuccessPacket());

    public static readonly ProtocolSerializer<ClientboundEncryptionSuccessPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
