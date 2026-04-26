using Codon.Binary;
using Codon.Optionals;
using ExonBreak.Protocol.Types.Text;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason DenyReason, Optional<TextComponent> Custom) : IPacket
{
    public ClientboundDenyLoginPacket(Reason DenyReason) : this(DenyReason, Optional.Empty<TextComponent>()) {}

    public static readonly BinaryCodec<ClientboundDenyLoginPacket> CODEC = BinaryCodec.Of
    (
        BinaryCodec.Enum<Reason>(), p => p.DenyReason,
        TextComponent.CODEC.Optional(), p => p.Custom,
        (reason, custom) => new ClientboundDenyLoginPacket(reason, custom)
    );

    public static readonly ProtocolSerializer<ClientboundDenyLoginPacket> SERIALIZER = Serializers.FromCodec(CODEC);

    public enum Reason
    {
        ProtocolVersionMismatch,
        NotWhitelist,
        Banned,
        InvalidName,
        MaxPlayersOnline,
        EncryptionError,
        SyncingError,
        Custom,
    }
}
