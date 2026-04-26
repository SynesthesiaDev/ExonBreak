using DotNetty.Codecs;
using ExonBreak.Protocol.Packets;
using Faster.Map.Core;

namespace ExonBreak.Protocol.Registry;

public class PacketRegistry
{
    private int idCounter;

    private readonly BlitzMap<int, IPacketEntry> idToEntry = new BlitzMap<int, IPacketEntry>();
    private readonly BlitzMap<Type, int> typeToId = new BlitzMap<Type, int>();

    public void Register<T>(ProtocolSerializer<T> serializer, Action<T, PacketContext>? handler = null) where T : IProtocolObject
    {
        var entry = new PacketEntry<T>(serializer, handler);
        var id = idCounter++;
        idToEntry[id] = entry;
        typeToId[typeof(T)] = id;
    }

    public void AddHandler<T>(Action<T, PacketContext> handler) where T : IProtocolObject
    {
        var a = idToEntry[GetId<T>()] as PacketEntry<T>;
        a?.Handler = handler;
    }

    public int GetId<T>() => GetId(typeof(T));

    public int GetId(Type type) => typeToId[type];

    public ProtocolSerializer<T> GetSerializer<T>() where T : IProtocolObject => (ProtocolSerializer<T>)idToEntry[typeToId[typeof(T)]].Serializer;

    public IProtocolSerializer GetSerializer(Type type) => (IProtocolSerializer)idToEntry[typeToId[type]].Serializer;

    public void ProcessPacket(WrappedPacket wrappedPacket, PacketContext context)
    {
        if (!idToEntry.Get(wrappedPacket.Id, out var entry)) throw new DecoderException($"Unknown packet with ID '{wrappedPacket.Id}'");

        var packet = ((IProtocolSerializer)entry.Serializer).Read(wrappedPacket.Data);
        entry.Handle(packet, context);
    }

    internal class PacketEntry<T>(ProtocolSerializer<T> serializer, Action<T, PacketContext>? handler) : IPacketEntry where T : IProtocolObject
    {
        public ProtocolSerializer<T> TypedSerializer => serializer;

        public object Serializer => serializer;

        public Action<T, PacketContext>? Handler { get; set; } = handler;

        public void Handle(object packet, PacketContext context) => Handler?.Invoke((T)packet, context);
    }
}
