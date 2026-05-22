using Codon.Binary;

namespace ExonBreak.Protocol.Types.Text;

public record TextColor(int PackedColor) : TextType
{
    public override TagType Type => TagType.Color;

    public static readonly IBinaryCodec<TextColor> CODEC = BinaryCodecs.For<TextColor>()
        .Field(BinaryCodecs.INT, p => p.PackedColor)
        .Build(packed => new TextColor(packed));

}
