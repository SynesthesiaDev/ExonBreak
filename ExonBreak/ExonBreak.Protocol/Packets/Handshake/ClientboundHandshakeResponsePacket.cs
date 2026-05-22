using Codon.Binary;
using ExonBreak.Protocol.Types.Text;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ClientboundHandshakeResponsePacket(int ServerProtocolVersion, ClientboundHandshakeResponsePacket.Status ServerStatus) : IPacket
{
    public static readonly IBinaryCodec<ClientboundHandshakeResponsePacket> CODEC = BinaryCodecs.For<ClientboundHandshakeResponsePacket>()
        .Field(BinaryCodecs.VAR_INT, p => p.ServerProtocolVersion)
        .Field(Status.CODEC, p => p.ServerStatus)
        .Build((version, status) => new ClientboundHandshakeResponsePacket(version, status));

    public static readonly ProtocolSerializer<ClientboundHandshakeResponsePacket> SERIALIZER = Serializers.FromCodec(CODEC);

    public record Status(
        string Title,
        TextComponent Subtitle,
        bool HasWhitelist,
        int OnlinePlayers,
        int Expeditions
    )
    {
        public static readonly IBinaryCodec<Status> CODEC = BinaryCodecs.For<Status>()
            .Field(BinaryCodecs.STRING, s => s.Title)
            .Field(TextComponent.CODEC, s => s.Subtitle)
            .Field(BinaryCodecs.BOOLEAN, s => s.HasWhitelist)
            .Field(BinaryCodecs.VAR_INT, s => s.OnlinePlayers)
            .Field(BinaryCodecs.VAR_INT, s => s.Expeditions)
            .Build((title, subtitle, whitelist, online, expeditions) => new Status(title, subtitle, whitelist, online, expeditions));
    }
}
