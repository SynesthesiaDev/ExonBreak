using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Server.Protocol.Netty;

public class OutboundServerPacketEncoder : MessageToMessageEncoder<IPacket>
{
    protected override void Encode(IChannelHandlerContext context, IPacket message, List<object> output)
    {
        var type = message.GetType();
        var id = DedicatedServer.CLIENTBOUND_PACKET_REGISTRY.GetId(type);
        var serializer = DedicatedServer.CLIENTBOUND_PACKET_REGISTRY.GetSerializer(type);

        var buffer = context.Allocator.Buffer();
        serializer.Write(buffer, message);

        output.Add(new WrappedPacket(id, buffer));
    }
}
