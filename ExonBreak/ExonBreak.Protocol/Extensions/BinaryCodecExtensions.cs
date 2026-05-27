using Codon.Binary;
using DotNetty.Buffers;

namespace ExonBreak.Protocol.Extensions;

public static class BinaryCodecExtensions
{
    public static IBinaryCodec<T> CustomCodec<T>(Action<IByteBuffer, T> encode, Func<IByteBuffer, T> decode) => new CustomBinaryCodec<T>(encode, decode);
    public class CustomBinaryCodec<T>(Action<IByteBuffer, T> encode, Func<IByteBuffer, T> decode) : IBinaryCodec<T>
    {
        public void Write(IByteBuffer buffer, T value) => encode.Invoke(buffer, value);
        public T Read(IByteBuffer buffer) => decode.Invoke(buffer);
    }

    public static IBinaryCodec<TBase> Cast<TBase, TDerived>(this IBinaryCodec<TDerived> codec)
        where TDerived : class, TBase
    {
        return new BinaryCodecDefinitions.BinaryCodecP1<TDerived, TBase>(
            codec,
            baseVal => (TDerived)baseVal!,
            derivedVal => (TBase)derivedVal
        );
    }
}
