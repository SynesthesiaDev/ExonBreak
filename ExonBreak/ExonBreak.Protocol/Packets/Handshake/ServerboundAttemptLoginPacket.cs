using Codon.Binary;
using ExonBreak.Protocol.Types.Player;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ServerboundAttemptLoginPacket(PlayerInfo PlayerInfo) : IPacket
{
    public static readonly IBinaryCodec<ServerboundAttemptLoginPacket> CODEC = BinaryCodecs.For<ServerboundAttemptLoginPacket>()
        .Field(PlayerInfo.CODEC, p => p.PlayerInfo)
        .Build(player => new ServerboundAttemptLoginPacket(player));

    public static readonly ProtocolSerializer<ServerboundAttemptLoginPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
