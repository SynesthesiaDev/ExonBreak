using Codon.Codec;

namespace ExonBreak.Online.Common.Boostrap;

public record BootstrapResponse(
    List<PinnedMessage> PinnedMessages,
    BootstrapResponse.ActiveEvent Event,
    BootstrapResponse.GameVersion LatestGameVersion
)
{
    public static readonly StructCodec<BootstrapResponse> STRUCT_CODEC = StructCodec.For<BootstrapResponse>()
        .Field("pinned_messages", PinnedMessage.STRUCT_CODEC.List(), b => b.PinnedMessages)
        .Field("active_event", Codecs.Enum<ActiveEvent>(), b => b.Event)
        .Field("latest_game_version", GameVersion.STRUCT_CODEC, b => b.LatestGameVersion)
        .Build((pinned, events, latestVersion) => new BootstrapResponse(pinned, events, latestVersion));

    public record GameVersion(string Version, string ReleaseUrl, long ReleaseDate)
    {
        public static readonly StructCodec<GameVersion> STRUCT_CODEC = StructCodec.For<GameVersion>()
            .Field("version", Codecs.STRING, g => g.Version)
            .Field("release_url", Codecs.STRING, g => g.Version)
            .Field("release_date", Codecs.LONG, g => g.ReleaseDate)
            .Build((version, release, date) => new GameVersion(version, release, date));
    }

    public enum ActiveEvent
    {
        None,
        Christmas,
        Halloween,
        NewYear,
        Valentine,
        Easter,
        Anniversary
    }
}