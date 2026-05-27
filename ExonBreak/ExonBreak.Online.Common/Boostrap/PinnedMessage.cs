using Codon.Codec;

namespace ExonBreak.Online.Common.Boostrap;

public record PinnedMessage(string Message, PinnedMessage.Type MessageType)
{
    public static readonly StructCodec<PinnedMessage> STRUCT_CODEC = StructCodec.For<PinnedMessage>()
        .Field("message", Codecs.STRING, p => p.Message)
        .Field("type", Codecs.Enum<Type>(), p => p.MessageType)
        .Build((message, type) => new PinnedMessage(message, type));

    public enum Type
    {
        Information,
        Announcement,
        Warning,
        BigRedScaryMessage
    }
}