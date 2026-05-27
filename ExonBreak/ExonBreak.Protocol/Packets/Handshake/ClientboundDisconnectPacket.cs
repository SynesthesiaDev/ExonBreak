using Codon.Binary;
using Codon.Optionals;
using ExonBreak.Protocol.Types.Text;

namespace ExonBreak.Protocol.Packets.Handshake;

public record ClientboundDisconnectPacket(ClientboundDisconnectPacket.Reason DenyReason, Optional<FormattedText> Custom) : IPacket
{
    public ClientboundDisconnectPacket(Reason DenyReason) : this(DenyReason, Optional.Empty<FormattedText>()) { }

    public static readonly IBinaryCodec<ClientboundDisconnectPacket> CODEC = BinaryCodecs.For<ClientboundDisconnectPacket>()
        .Field(BinaryCodecs.Enum<Reason>(), p => p.DenyReason)
        .Field(FormattedText.CODEC.Optional(), p => p.Custom)
        .Build((reason, custom) => new ClientboundDisconnectPacket(reason, custom));

    public static readonly ProtocolSerializer<ClientboundDisconnectPacket> SERIALIZER = Serializers.FromCodec(CODEC);

    public enum Reason
    {
        ProtocolVersionMismatch,
        NotWhitelist,
        Banned,
        InvalidName,
        MaxPlayersOnline,
        EncryptionError,
        IdentityMismatch,
        SyncingError,
        Custom,
    }
}
