using Codon.Binary;
using ExonBreak.Protocol.Types.Player;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ServerboundAttemptLoginPacket(PlayerInfo PlayerInfo) : IPacket
{
    public static readonly BinaryCodec<ServerboundAttemptLoginPacket> CODEC = BinaryCodec.Of
    (
        PlayerInfo.CODEC, p => p.PlayerInfo,
        (player) => new ServerboundAttemptLoginPacket(player)
    );

    public static readonly ProtocolSerializer<ServerboundAttemptLoginPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
