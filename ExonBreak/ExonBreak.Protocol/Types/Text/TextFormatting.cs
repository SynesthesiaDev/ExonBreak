using Codon.Binary;

namespace ExonBreak.Protocol.Types.Text;

public record TextFormatting(TextFormatting.Tag Enable = 0, TextFormatting.Tag Disable = 0) : TextType
{
    public new static readonly BinaryCodec<TextFormatting> CODEC = BinaryCodec.Of
    (
        BinaryCodec.Flags<Tag>(), p => p.Enable,
        BinaryCodec.Flags<Tag>(), p => p.Disable,
        (enable, disable) => new TextFormatting(enable, disable)
    );

    public override TagType Type => TagType.Content;

    [Flags]
    public enum Tag : byte
    {
        None = 0,
        Bold = 1,
        Italic = 2,
        Strikethrough = 4,
        Underlined = 8,
    }

}
