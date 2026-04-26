using Codon.Binary;
using ExonBreak.Protocol.Types.Player;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ServerboundHandshakeRequestPacket(int ServerProtocolVersion, PlayerInfo PlayerInfo) : IProtocolObject
{
    public static readonly BinaryCodec<ServerboundHandshakeRequestPacket> CODEC = BinaryCodec.Of
    (
        BinaryCodec.VAR_INT, p => p.ServerProtocolVersion,
        PlayerInfo.CODEC, p => p.PlayerInfo,
        (pv, player) => new ServerboundHandshakeRequestPacket(pv, player)
    );

    public static readonly ProtocolSerializer<ServerboundHandshakeRequestPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
