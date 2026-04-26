using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Server.Protocol.Netty;

public class InboundWrappedPacketDecoder : ByteToMessageDecoder
{
    protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
    {
        try
        {
            var packet = WrappedPacket.CODEC.Read(input);
            output.Add(packet);
        }
        catch (Exception exception)
        {
            context.FireExceptionCaught(exception);
        }
    }
}
