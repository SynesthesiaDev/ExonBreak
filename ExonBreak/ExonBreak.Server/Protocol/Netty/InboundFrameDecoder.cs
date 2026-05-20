using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace ExonBreak.Server.Protocol.Netty;

public class InboundFrameDecoder : ByteToMessageDecoder
{
    private const int header_length = sizeof(int);

    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        if (input.ReadableBytes < header_length) return;

        input.MarkReaderIndex();
        var length = input.ReadInt();

        if (length is <= 0 or > 1024 * 1024)
        {
            throw new CorruptedFrameException($"Illegal packet length: {length}");
        }

        if (input.ReadableBytes < length)
        {
            input.ResetReaderIndex();
            return;
        }

        output.Add(input.ReadRetainedSlice(length));
    }
}
