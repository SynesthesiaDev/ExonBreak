using Codon.Binary;
using DotNetty.Buffers;

namespace ExonBreak.Protocol.Packets;

public record WrappedPacket(int Id, IByteBuffer Data) : IProtocolObject
{
    public static readonly BinaryCodec<WrappedPacket> CODEC = BinaryCodec.Of
    (
        BinaryCodec.VAR_INT, p => p.Id,
        BinaryCodec.BYTE_BUFFER, p => p.Data,
        (id, data) => new WrappedPacket(id, data)
    );

    public static readonly ProtocolSerializer<WrappedPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
