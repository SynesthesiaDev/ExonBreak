using Codon.Binary;
using Codon.Optionals;
using ExonBreak.Protocol.Types.Text;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason DenyReason, Optional<TextComponent> Custom) : IPacket
{
    public ClientboundDenyLoginPacket(Reason DenyReason) : this(DenyReason, Optional.Empty<TextComponent>()) { }

    public static readonly IBinaryCodec<ClientboundDenyLoginPacket> CODEC = BinaryCodecs.For<ClientboundDenyLoginPacket>()
        .Field(BinaryCodecs.Enum<Reason>(), p => p.DenyReason)
        .Field(TextComponent.CODEC.Optional(), p => p.Custom)
        .Build((reason, custom) => new ClientboundDenyLoginPacket(reason, custom));

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
