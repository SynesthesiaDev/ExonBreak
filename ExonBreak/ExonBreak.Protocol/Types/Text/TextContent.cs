using Codon.Binary;

namespace ExonBreak.Protocol.Types.Text;

public record TextContent(string Content) : TextType
{
    public static readonly IBinaryCodec<TextContent> CODEC = BinaryCodecs.For<TextContent>()
        .Field(BinaryCodecs.STRING, p => p.Content)
        .Build(packed => new TextContent(packed));

    public override TagType Type => TagType.Content;
}
