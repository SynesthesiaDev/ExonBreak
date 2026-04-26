using DotNetty.Transport.Channels;

namespace ExonBreak.Protocol;

public class PacketContext(IChannelHandlerContext channelHandlerContext)
{
    public IChannelHandlerContext ChannelHandlerContext { get; } = channelHandlerContext;

    public void SendPacket(IPacket packet)
    {
        ChannelHandlerContext.WriteAndFlushAsync(packet);
    }
}
