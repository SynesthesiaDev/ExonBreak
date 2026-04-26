using ExonBreak.Common.Text;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Registry;
using ExonBreak.Server.Config;
using Serilog;

namespace ExonBreak.Server;

public class DedicatedServer
{
    public ServerboundPacketRegistry ServerboundPacketRegistry = new ServerboundPacketRegistry();
    public static int OnlinePlayers { get; set; } = 0;
    public static int MaxPlayers { get; set; } = 0;
    public static int Expeditions { get; set; } = 0;

    public static bool IsInternal { get; private set; } = false;

    public static ClientboundHandshakeResponsePacket.Status CachedStatus = null!;

    public static readonly string PATH = AppDomain.CurrentDomain.BaseDirectory;

    public void InvalidateCachedStatus()
    {
        CachedStatus = new ClientboundHandshakeResponsePacket.Status(
            ServerConfig.Title,
            TextFormatter.Parse(ServerConfig.Subtitle),
            ServerConfig.WhitelistEnabled,
            OnlinePlayers,
            Expeditions
        );
    }

    public DedicatedServer(bool isInternal)
    {
        IsInternal = isInternal;
    }

    public void Run()
    {
        Log.Debug("Loading Exon Break {Type} server..", IsInternal ? "internal" : "dedicated");
        ServerConfig.Load();
        InvalidateCachedStatus();
    }
}
