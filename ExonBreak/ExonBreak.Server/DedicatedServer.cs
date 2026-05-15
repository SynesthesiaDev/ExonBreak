using ExonBreak.Common.Text;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Registry;
using ExonBreak.Server.Config;
using ExonBreak.Server.Protocol;
using ExonBreak.Server.Protocol.Netty;
using Serilog;

namespace ExonBreak.Server;

public class DedicatedServer
{
    public static readonly ServerboundPacketRegistry SERVERBOUND_PACKET_REGISTRY = new ServerboundPacketRegistry();
    public static readonly ClientboundPacketRegistry CLIENTBOUND_PACKET_REGISTRY = new ClientboundPacketRegistry();
    public static int OnlinePlayers { get; set; } = 0;
    public static int MaxPlayers { get; set; } = 0;
    public static int Expeditions { get; set; } = 0;
    public static bool IsInternal { get; private set; }

    public static ClientboundHandshakeResponsePacket.Status CachedStatus = null!;

    public static readonly string PATH = AppDomain.CurrentDomain.BaseDirectory;

    public static void InvalidateCachedStatus()
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
        var type = IsInternal ? "internal" : "dedicated";
        Log.Debug($"Loading Exon Break {type} server..");

        ServerConfig.Load();
        InvalidateCachedStatus();

        ServerPacketHandlers.RegisterHandlers(SERVERBOUND_PACKET_REGISTRY);

        var netty = new NettyServer();
        _ = netty.StartAsync();

        Console.ReadLine();
    }
}
