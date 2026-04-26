using Codon.Binary;
using DotNetty.Buffers;

namespace ExonBreak.Protocol;

public static class Serializers
{
    public static ProtocolSerializer<T> Manual<T>(Func<IByteBuffer, T> read, Action<T, IByteBuffer> write) where T : IProtocolObject => new(read, write);

    public static ProtocolSerializer<T> FromCodec<T>(BinaryCodec<T> binaryCodec) where T : IProtocolObject
    {
        return new ProtocolSerializer<T>(binaryCodec.Read, (value, buffer) => binaryCodec.Write(buffer, value));
    }
}
