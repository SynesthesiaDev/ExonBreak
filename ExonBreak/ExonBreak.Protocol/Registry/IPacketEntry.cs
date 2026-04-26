namespace ExonBreak.Protocol.Registry;

internal interface IPacketEntry
{
    void Handle(object packet, PacketContext context);

    object Serializer { get; }
}
