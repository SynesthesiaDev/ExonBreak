using Codon.Binary;
using ExonBreak.Protocol.Types.Text;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ClientboundHandshakeResponsePacket(int ServerProtocolVersion, ClientboundHandshakeResponsePacket.Status ServerStatus) : IPacket
{
    public static readonly BinaryCodec<ClientboundHandshakeResponsePacket> CODEC = BinaryCodec.Of
    (
        BinaryCodec.VAR_INT, p => p.ServerProtocolVersion,
        Status.CODEC, p => p.ServerStatus,
        (version, status) => new ClientboundHandshakeResponsePacket(version, status)
    );

    public static readonly ProtocolSerializer<ClientboundHandshakeResponsePacket> SERIALIZER = Serializers.FromCodec(CODEC);

    public record Status(
        string Title,
        TextComponent Subtitle,
        bool HasWhitelist,
        int OnlinePlayers,
        int Expeditions
    )
    {
        public static readonly BinaryCodec<Status> CODEC = BinaryCodec.Of
        (
            BinaryCodec.STRING, s => s.Title,
            TextComponent.CODEC, s => s.Subtitle,
            BinaryCodec.BOOLEAN, s => s.HasWhitelist,
            BinaryCodec.VAR_INT, s => s.OnlinePlayers,
            BinaryCodec.VAR_INT, s => s.Expeditions,
            (title, subtitle, whitelist, online, expeditions) => new Status(title, subtitle, whitelist, online, expeditions)
        );
    }
}
