using Codon.Binary;

namespace ExonBreak.Protocol.Extensions;

public static class BinaryCodecExtensions
{
    public static BinaryCodec<TBase> Cast<TBase, TDerived>(this BinaryCodec<TDerived> codec)
        where TDerived : class, TBase
    {
        return BinaryCodec.Of(
            codec,
            baseVal => (TDerived)baseVal,
            derivedVal => (TBase)derivedVal
        );
    }
}
