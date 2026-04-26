using Codon.Binary;

namespace ExonBreak.Protocol.Types.Text;

public record TextColor(int PackedColor) : TextType
{
    public override TagType Type => TagType.Color;

    public static readonly BinaryCodec<TextColor> CODEC = BinaryCodec.Of
    (
        BinaryCodec.INT, p => p.PackedColor,
        packed => new TextColor(packed)
    );

}
