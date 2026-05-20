using System.Threading.Tasks;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Registry;
using ExonBreak.Protocol.Types.Player;
using ExonBreak.Server.Protocol;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class GameClient(PlayerInfo playerInfo, string address = SharedConstants.DEFAULT_IP_ADDRESS, int port = SharedConstants.DEFAULT_PORT)
{
    public static readonly ServerboundPacketRegistry SERVERBOUND_PACKET_REGISTRY = new ServerboundPacketRegistry(s => Logger.Log(s, LoggingTarget.Network), ProtocolSide.Client);
    public static readonly ClientboundPacketRegistry CLIENTBOUND_PACKET_REGISTRY = new ClientboundPacketRegistry(s => Logger.Log(s, LoggingTarget.Network), ProtocolSide.Client);

    public readonly PlayerInfo PlayerInfo = playerInfo;

    public NettyClient NettyClient = null!;

    public async Task Connect()
    {
        Logger.Log("Trying to connect..");
        ClientPacketHandlers.RegisterHandlers(CLIENTBOUND_PACKET_REGISTRY);
        NettyClient = new NettyClient(address, port, this);

        await NettyClient.ConnectAsync();
    }
}
