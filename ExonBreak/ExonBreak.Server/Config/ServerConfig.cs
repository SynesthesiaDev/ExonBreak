using System.Text.Json;
using Codon.Codec;
using Codon.Codec.Json;
using Codon.Codec.Versioned;
using ExonBreak.Protocol;
using Serilog;

namespace ExonBreak.Server.Config;

public static class ServerConfig
{
    private static readonly Codec<Guid> guid_codec = Codecs.STRING.Transform(Guid.Parse, guid => guid.ToString());
    private static readonly string config_path = Path.Join(DedicatedServer.PATH, "config.json");

    private static ServerConfigFile current = null!;
    private static bool ranMigration;

    public static string Ip => current.Ip;
    public static int Port => current.Port;
    public static int MaxPlayers => current.MaxPlayers;
    public static string Title => current.Title;
    public static string Subtitle => current.Subtitle;

    public static bool WhitelistEnabled => current.Whitelist.Enabled;
    public static IReadOnlyList<Guid> WhitelistedPlayers => current.Whitelist.Players.AsReadOnly();
    public static IReadOnlyList<Guid> BannedPlayers => current.BannedPlayers.AsReadOnly();

    public static void WhitelistPlayer(Guid player)
    {
        current.Whitelist.Players.Add(player);
        Write();
    }

    public static void UpdateStatus(string title, string subtitle)
    {
        current = current with { Title = title };
        current = current with { Subtitle = subtitle };
    }

    public static readonly ServerConfigFile DEFAULT = new ServerConfigFile
    (
        Ip: SharedConstants.DEFAULT_IP_ADDRESS,
        Port: SharedConstants.DEFAULT_PORT,
        MaxPlayers: 4,
        Whitelist: Whitelist.DEFAULT,
        BannedPlayers: [],
        Title: "Exon Break Server",
        Subtitle: "The default Exon Break server"
    );

    public static void Load()
    {
        Log.Debug("Loading config file at {Path}..", config_path);
        try
        {
            if (!File.Exists(config_path))
            {
                Log.Debug("Server config file doesn't exist yet, creating one..");
                File.Create(config_path).Close();
                current = DEFAULT;
                Write();
            }

            var readJson = File.ReadAllText(config_path);
            var readConfig = ServerConfigFile.CODEC.Decode(JsonTranscoder.INSTANCE, JsonDocument.Parse(readJson).RootElement);
            current = readConfig;
            Log.Information("Loaded server config file!");
            if (ranMigration) Write();
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to read config file");
        }
    }

    public static void Write()
    {
        var json = ServerConfigFile.CODEC.Encode(JsonTranscoder.INSTANCE, current).ToString();
        File.WriteAllText(config_path, json);
    }

    public record ServerConfigFile(
        string Ip,
        int Port,
        int MaxPlayers,
        Whitelist Whitelist,
        List<Guid> BannedPlayers,
        string Title,
        string Subtitle
    )
    {
        public static readonly StructCodec<ServerConfigFile> RAW_CODEC = StructCodec.For<ServerConfigFile>()
            .Field("ip", Codecs.STRING, c => c.Ip)
            .Field("port", Codecs.INT, c => c.Port)
            .Field("max_players", Codecs.INT, c => c.MaxPlayers)
            .Field("whitelist", Whitelist.CODEC, c => c.Whitelist)
            .Field("banned_players", guid_codec.List(), c => c.BannedPlayers)
            .Field("title", Codecs.STRING, c => c.Title)
            .Field("subtitle", Codecs.STRING, c => c.Subtitle)
            .Build((ip, port, max, whitelist, banned, title, subtitle) => new ServerConfigFile(ip, port, max, whitelist, banned, title, subtitle));

        // Schema version changes:
        // 2 - Added `BannedPlayers`
        public static readonly VersionedStructCodec<ServerConfigFile> CODEC = new VersionedStructCodec<ServerConfigFile>
        {
            CurrentSchemaVersion = 2,
            InnerCodec = RAW_CODEC,
            SchemaMigrationRegistry = SchemaMigrationRegistry.Builder()
                .For<JsonElement>(migrations =>
                {
                    migrations.Add(2, (transcoder, _, output) =>
                    {
                        Log.Debug("Migrating server config to schema version 2..");
                        ranMigration = true;
                        output.Put("banned_players", transcoder.EncodeList(0).Build());
                    });
                })
        };
    }

    public record Whitelist(
        bool Enabled,
        List<Guid> Players
    )
    {
        public static readonly Whitelist DEFAULT = new Whitelist(false, []);

        public static readonly Codec<Whitelist> CODEC = StructCodec.For<Whitelist>()
            .Field("enabled", Codecs.BOOLEAN, w => w.Enabled)
            .Field("players", guid_codec.List(), w => w.Players)
            .Build((enabled, players) => new Whitelist(enabled, players));
    }
}
