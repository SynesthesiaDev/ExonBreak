using System.Text;
using Codon.Binary;
using DotNetty.Buffers;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Protocol.Types.Text;

public record TextComponent(List<TextType> Instructions) : IProtocolObject
{
    public static readonly BinaryCodec<TextComponent> CODEC = BinaryCodec.Of(TextType.CODEC.List(), p => p.Instructions, tt => new TextComponent(tt));

    public static readonly IProtocolSerializer SERIALIZER = Serializers.FromCodec(CODEC);

    public byte[] ToBytes()
    {
        var buffer = Unpooled.Buffer();
        SERIALIZER.Write(buffer, this);
        return buffer.Array;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var inst in Instructions)
        {
            if (inst is TextContent content) sb.Append(content.Content);
        }

        return sb.ToString();
    }
}
