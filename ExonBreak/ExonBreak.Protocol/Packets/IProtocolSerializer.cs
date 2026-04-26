using DotNetty.Buffers;

namespace ExonBreak.Protocol.Packets;

public interface IProtocolSerializer
{
    object Read(IByteBuffer data);
    void Write(IByteBuffer buffer, object value);
}
