using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace ExonBreak.Server.Protocol.Netty;

public class OutboundLengthPrepender : MessageToMessageEncoder<IByteBuffer>
{
    private const int header_length = sizeof(int);

    protected override void Encode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
    {
        var bodyLength = message.ReadableBytes;

        var headerBuf = context.Allocator.Buffer(header_length);
        headerBuf.WriteInt(bodyLength);

        output.Add(headerBuf);
        output.Add(message.Retain());
    }
}
