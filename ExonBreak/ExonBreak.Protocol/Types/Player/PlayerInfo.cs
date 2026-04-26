using Codon.Binary;

namespace ExonBreak.Protocol.Types.Player;

public record PlayerInfo(int ClientProtocolVersion, Guid Id, string Username, string Pronouns, Platform Platform) : IProtocolObject
{
    public static readonly IReadOnlyList<SpecialTag> SPECIAL_TAGS =
    [
        new SpecialTag("Dev", -3541249, 239818751, []),
        new SpecialTag("Friend", -3152641, 1460547071, []),
    ];

    public static readonly BinaryCodec<PlayerInfo> CODEC = BinaryCodec.Of
    (
        BinaryCodec.VAR_INT, p => p.ClientProtocolVersion,
        BinaryCodec.GUID, p => p.Id,
        BinaryCodec.String(16), p => p.Username,
        BinaryCodec.String(24), p => p.Pronouns,
        BinaryCodec.Enum<Platform>(), p => p.Platform,
        (pv, id, username, pronouns, platform) => new PlayerInfo(pv, id, username, pronouns, platform)
    );

    public bool IsDev => SPECIAL_TAGS[0].Players.Contains(Id);
    public bool IsFriendOfDev => SPECIAL_TAGS[1].Players.Contains(Id);

    public record SpecialTag(string Name, int Color, int TextColor, List<Guid> Players);
}
