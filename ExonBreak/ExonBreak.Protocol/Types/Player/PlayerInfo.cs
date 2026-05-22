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
}
