using DotNetty.Buffers;

namespace ExonBreak.Online.Common.Serialization;

public class BinaryTranscoderContext(IByteBuffer buffer)
{
    public IByteBuffer Buffer { get; } = buffer;
}