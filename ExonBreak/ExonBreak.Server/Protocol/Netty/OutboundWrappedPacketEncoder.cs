using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Server.Protocol.Netty;

public class OutboundWrappedPacketEncoder : MessageToMessageEncoder<WrappedPacket>
{
    protected override void Encode(IChannelHandlerContext context, WrappedPacket message, List<object> output)
    {
        var outBuffer = context.Allocator.Buffer();
        WrappedPacket.CODEC.Write(outBuffer, message);
        output.Add(outBuffer);
    }
}
