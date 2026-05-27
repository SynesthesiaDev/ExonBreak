using Codon.Codec;

namespace ExonBreak.Online.Common.Boostrap;

public record BootstrapRequest
{
    public record PlayerInfo(string Name, Guid Guid, string GameVersion)
    {
        public static readonly StructCodec<PlayerInfo> STRUCT_CODEC = StructCodec.For<PlayerInfo>()
            .Field("name", Codecs.STRING, p => p.Name)
            .Field("guid", ExtraCodecs.GUID, p => p.Guid)
            .Field("game_version", Codecs.STRING, p => p.GameVersion)
            .Build((name, guid, version) => new PlayerInfo(name, guid, version));

    }
}