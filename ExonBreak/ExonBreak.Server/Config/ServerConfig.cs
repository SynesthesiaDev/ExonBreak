using System.Text.Json;
using Codon.Codec;
using Codon.Codec.Json;
using Serilog;

namespace ExonBreak.Server.Config;

public static class ServerConfig
{
    private static readonly Codec<Guid> guid_codec = Codecs.STRING.Transform(Guid.Parse, guid => guid.ToString());
    private static readonly string config_path = Path.Join(DedicatedServer.PATH, "config.json");

    private static ConfigFile current = null!;

    public static string Ip => current.Ip;
    public static int Port => current.Port;
    public static int MaxPlayers => current.MaxPlayers;
    public static string Title => current.Title;
    public static string Subtitle => current.Subtitle;

    public static bool WhitelistEnabled => current.Whitelist.Enabled;
    public static IReadOnlyList<Guid> WhitelistedPlayers => current.Whitelist.Players.AsReadOnly();

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

    public static readonly ConfigFile DEFAULT = new ConfigFile
    (
        Ip: "0.0.0.0",
        Port: 58730,
        MaxPlayers: 4,
        Whitelist: Whitelist.DEFAULT,
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
            var readConfig = ConfigFile.CODEC.Decode(JsonTranscoder.INSTANCE, JsonDocument.Parse(readJson).RootElement);
            current = readConfig;
            Log.Information("Loaded server config file!");
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Failed to read config file");
        }
    }

    public static void Write()
    {
        var json = ConfigFile.CODEC.Encode(JsonTranscoder.INSTANCE, current).ToString();
        File.WriteAllText(config_path, json);
    }

    public record ConfigFile(
        string Ip,
        int Port,
        int MaxPlayers,
        Whitelist Whitelist,
        string Title,
        string Subtitle
    )
    {
        public static readonly Codec<ConfigFile> CODEC = StructCodec.Of
        (
            "ip", Codecs.STRING, c => c.Ip,
            "port", Codecs.INT, c => c.Port,
            "max_players", Codecs.INT, c => c.MaxPlayers,
            "whitelist", Whitelist.CODEC, c => c.Whitelist,
            "title", Codecs.STRING, c => c.Title,
            "subtitle", Codecs.STRING, c => c.Subtitle,
            (ip, port, max, whitelist, title, subtitle) => new ConfigFile(ip, port, max, whitelist, title, subtitle)
        );
    }

    public record Whitelist(
        bool Enabled,
        List<Guid> Players
    )
    {
        public static readonly Whitelist DEFAULT = new Whitelist(false, []);

        public static readonly Codec<Whitelist> CODEC = StructCodec.Of
        (
            "enabled", Codecs.BOOLEAN, w => w.Enabled,
            "players", guid_codec.List(), w => w.Players,
            (enabled, players) => new Whitelist(enabled, players)
        );
    }
}
