using Codon.Binary;
using DotNetty.Buffers;

namespace ExonBreak.Protocol.Packets;

public record WrappedPacket(int Id, IByteBuffer Data) : IProtocolObject
{
    public static readonly IBinaryCodec<WrappedPacket> CODEC = BinaryCodecs.For<WrappedPacket>()
        .Field(BinaryCodecs.VAR_INT, p => p.Id)
        .Field(BinaryCodecs.BYTE_BUFFER, p => p.Data)
        .Build((id, data) => new WrappedPacket(id, data));

    public static readonly ProtocolSerializer<WrappedPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
