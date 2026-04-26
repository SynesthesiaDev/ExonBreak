using Codon.Binary;
using ExonBreak.Protocol.Extensions;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Protocol.Types.Text;

public abstract record TextType : IProtocolObject
{
    public abstract TagType Type { get; }

    public static readonly BinaryCodec<TextType> CODEC = new BinaryCodecs.UnionBinaryCodec<TextType, TagType>(
        BinaryCodec.Enum<TagType>(),
        value => value.Type,
        tag => tag switch
        {
            TagType.Color => TextColor.CODEC.Cast<TextType, TextColor>(),
            TagType.Content => TextContent.CODEC.Cast<TextType, TextContent>(),
            TagType.Formatting => TextFormatting.CODEC.Cast<TextType, TextFormatting>(),
            _ => throw new ArgumentOutOfRangeException(nameof(tag))
        }
    );

    public static readonly IProtocolSerializer SERIALIZER = Serializers.FromCodec(CODEC);

    public enum TagType
    {
        Color,
        Content,
        Formatting
    }
}
