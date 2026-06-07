using System;
using System.Threading.Tasks;
using ExonBreak.Common.Dispatchers;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Registry;
using ExonBreak.Protocol.Types.Player;
using ExonBreak.Server.Protocol;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol;

public class GameClient(PlayerInfo playerInfo, string address = SharedConstants.DEFAULT_IP_ADDRESS, int port = SharedConstants.DEFAULT_PORT) : IDisposable
{
    public static readonly ServerboundPacketRegistry SERVERBOUND_PACKET_REGISTRY = new ServerboundPacketRegistry(s => Logger.Log(s, LoggingTarget.Network), ProtocolSide.Client);
    public static readonly ClientboundPacketRegistry CLIENTBOUND_PACKET_REGISTRY = new ClientboundPacketRegistry(s => Logger.Log(s, LoggingTarget.Network), ProtocolSide.Client);

    public readonly PlayerInfo PlayerInfo = playerInfo;

    public NettyClient NettyClient = null!;

    public readonly EventDispatcher<GameClient> OnDisconnected = new EventDispatcher<GameClient>();
    public readonly EventDispatcher<GameClient> OnConnected = new EventDispatcher<GameClient>();
    private string address = address;

    public async Task Connect()
    {
        Logger.Log("Trying to connect..");
        ClientPacketHandlers.RegisterHandlers(CLIENTBOUND_PACKET_REGISTRY);
        if (address == "localhost") address = "127.0.0.1";
        NettyClient = new NettyClient(address, port, this);

        await NettyClient.ConnectAsync();
    }

    public void Disconnect()
    {
        NettyClient?.Dispose();
    }

    public void Dispose()
    {
        NettyClient.Dispose();
        OnDisconnected.Dispose();
        OnConnected.Dispose();
    }
}
