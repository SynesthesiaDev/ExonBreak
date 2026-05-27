using DotNetty.Transport.Channels;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Protocol;

public abstract class PlayerConnection : SimpleChannelInboundHandler<WrappedPacket>
{
    public abstract IChannelHandlerContext ChannelHandlerContext { get; set; }
}
