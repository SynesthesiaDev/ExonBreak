using DotNetty.Transport.Channels;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Protocol.Types;

public abstract class PacketHandler : SimpleChannelInboundHandler<WrappedPacket>
{
    public abstract void OnConnected(IChannel channel);
}
