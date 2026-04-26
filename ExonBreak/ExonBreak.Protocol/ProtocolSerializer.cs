using DotNetty.Buffers;
using ExonBreak.Protocol.Packets;

namespace ExonBreak.Protocol;

public record ProtocolSerializer<T>(
    Func<IByteBuffer, T> Read,
    Action<T, IByteBuffer> Write
) : IProtocolSerializer where T : IProtocolObject
{
    public T ReadTyped(IByteBuffer buffer) => Read.Invoke(buffer);

    public void WriteTyped(IByteBuffer buffer, T value) => Write.Invoke(value, buffer);

    object IProtocolSerializer.Read(IByteBuffer data) => ReadTyped(data);

    void IProtocolSerializer.Write(IByteBuffer buffer, object value) => WriteTyped(buffer, (T)value);
}
