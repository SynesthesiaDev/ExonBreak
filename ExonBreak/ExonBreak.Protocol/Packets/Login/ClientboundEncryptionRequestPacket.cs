using Codon.Binary;

namespace ExonBreak.Protocol.Packets.Login;

public record ClientboundEncryptionRequestPacket(byte[] ServerPublicKey, byte[] Challenge) : IPacket
{
    public static readonly IBinaryCodec<ClientboundEncryptionRequestPacket> CODEC = BinaryCodecs.For<ClientboundEncryptionRequestPacket>()
        .Field(BinaryCodecs.BYTE_ARRAY, c => c.ServerPublicKey)
        .Field(BinaryCodecs.ByteArray(32), c => c.Challenge)
        .Build((pub, challenge) => new ClientboundEncryptionRequestPacket(pub, challenge));

    public static readonly ProtocolSerializer<ClientboundEncryptionRequestPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
