using System.Security.Cryptography;
using Codon.Binary;

namespace ExonBreak.Protocol.Types.Player;

public record PlayerInfo(int ClientProtocolVersion, Guid Id, string Username, string Pronouns, Platform Platform) : IProtocolObject
{
    public static readonly IReadOnlyList<SpecialTag> SPECIAL_TAGS =
    [
        new SpecialTag("Dev", -3541249, 239818751, []),
        new SpecialTag("Friend", -3152641, 1460547071, []),
    ];

    public static readonly IBinaryCodec<PlayerInfo> CODEC = BinaryCodecs.For<PlayerInfo>()
        .Field(BinaryCodecs.VAR_INT, p => p.ClientProtocolVersion)
        .Field(BinaryCodecs.GUID, p => p.Id)
        .Field(BinaryCodecs.String(16), p => p.Username)
        .Field(BinaryCodecs.String(24), p => p.Pronouns)
        .Field(BinaryCodecs.Enum<Platform>(), p => p.Platform)
        .Build((pv, id, username, pronouns, platform) => new PlayerInfo(pv, id, username, pronouns, platform));

    public bool IsDev => SPECIAL_TAGS[0].Players.Contains(Id);
    public bool IsFriendOfDev => SPECIAL_TAGS[1].Players.Contains(Id);

    public record SpecialTag(string Name, int Color, int TextColor, List<Guid> Players);

    public static Guid DeriveGuid(byte[] publicKeyBytes)
    {
        var hash = SHA256.HashData(publicKeyBytes);
        var guidBytes = hash[..16];
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x40);
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);
        return new Guid(guidBytes);
    }
}
