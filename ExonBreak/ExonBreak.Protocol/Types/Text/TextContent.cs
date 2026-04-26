using Codon.Binary;

namespace ExonBreak.Protocol.Types.Text;

public record TextContent(string Content) : TextType
{
    public static readonly BinaryCodec<TextContent> CODEC = BinaryCodec.Of
    (
        BinaryCodec.STRING, p => p.Content,
        packed => new TextContent(packed)
    );

    public override TagType Type => TagType.Content;
}
