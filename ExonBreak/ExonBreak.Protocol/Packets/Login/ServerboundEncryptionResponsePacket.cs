using Codon.Binary;

namespace ExonBreak.Protocol.Packets.Login;

public record ServerboundEncryptionResponsePacket(byte[] ClientPublicKey, byte[] Signature, byte[] SharedSecret) : IPacket
{
    public static readonly IBinaryCodec<ServerboundEncryptionResponsePacket> CODEC = BinaryCodecs.For<ServerboundEncryptionResponsePacket>()
        .Field(BinaryCodecs.BYTE_ARRAY, c => c.ClientPublicKey)
        .Field(BinaryCodecs.BYTE_ARRAY, c => c.Signature)
        .Field(BinaryCodecs.BYTE_ARRAY, c => c.SharedSecret)
        .Build((pub, signature, secret) => new ServerboundEncryptionResponsePacket(pub, signature, secret));

    public static readonly ProtocolSerializer<ServerboundEncryptionResponsePacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
