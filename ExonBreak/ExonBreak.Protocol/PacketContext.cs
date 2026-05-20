using DotNetty.Transport.Channels;
using ExonBreak.Server.Protocol;

namespace ExonBreak.Protocol;

public class PacketContext(IChannelHandlerContext channelHandlerContext, Action<string> logFunction, ProtocolSide side)
{
    public IChannelHandlerContext ChannelHandlerContext { get; } = channelHandlerContext;

    public void SendPacket(IPacket packet)
    {
        logFunction.Invoke($"({side.ToString()}) ← {packet.GetType().Name}");
        ChannelHandlerContext.Channel.WriteAndFlushAsync(packet);
    }
}
