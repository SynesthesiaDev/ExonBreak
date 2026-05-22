using Codon.Binary;
using ExonBreak.Protocol.Types.Player;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ServerboundHandshakeRequestPacket(int ServerProtocolVersion, PlayerInfo PlayerInfo) : IPacket
{
    public static readonly IBinaryCodec<ServerboundHandshakeRequestPacket> CODEC = BinaryCodecs.For<ServerboundHandshakeRequestPacket>()
        .Field(BinaryCodecs.VAR_INT, p => p.ServerProtocolVersion)
        .Field(PlayerInfo.CODEC, p => p.PlayerInfo)
        .Build((pv, player) => new ServerboundHandshakeRequestPacket(pv, player));

    public static readonly ProtocolSerializer<ServerboundHandshakeRequestPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
