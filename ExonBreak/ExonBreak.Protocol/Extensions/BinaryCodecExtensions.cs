using Codon.Binary;

namespace ExonBreak.Protocol.Extensions;

public static class BinaryCodecExtensions
{
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
