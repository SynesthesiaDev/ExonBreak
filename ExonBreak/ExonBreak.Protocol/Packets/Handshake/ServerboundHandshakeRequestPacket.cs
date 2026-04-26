using Codon.Binary;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ServerboundHandshakeRequestPacket(int ServerProtocolVersion, Guid Id, string Username) : IProtocolObject
{
    public static readonly BinaryCodec<ServerboundHandshakeRequestPacket> CODEC = BinaryCodec.Of
    (
        BinaryCodec.VAR_INT, p => p.ServerProtocolVersion,
        BinaryCodec.GUID, p => p.Id,
        BinaryCodec.STRING, p => p.Username,
        (pv, id, username) => new ServerboundHandshakeRequestPacket(pv, id, username)
    );

    public static readonly ProtocolSerializer<ServerboundHandshakeRequestPacket> SERIALIZER = Serializers.FromCodec(CODEC);
}
