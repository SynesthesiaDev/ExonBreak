using ExonBreak.Game.Extensions;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets.Handshake;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol.Handlers;

public class ClientHandshakeHandlers
{
    public static void HandleHandshake(ClientboundHandshakeResponsePacket clientboundHandshakeResponsePacket, PacketContext packetContext)
    {
        Logger.Log($"Handshake response - {clientboundHandshakeResponsePacket.ServerProtocolVersion} ({clientboundHandshakeResponsePacket.ServerStatus})", LoggingTarget.Network);

        packetContext.SendPacket(new ServerboundAttemptLoginPacket(packetContext.PlayerConnection.AsClient().GameClient.PlayerInfo));
    }

    public static void HandleAttemptLoginAccept(ClientboundAcceptLoginPacket clientboundAcceptLoginPacket, PacketContext packetContext)
    {
        Logger.Log("Attempt login accept", LoggingTarget.Network);
    }

    public static void HandleAttemptLoginDeny(ClientboundDisconnectPacket packet, PacketContext packetContext)
    {
        Logger.Log($"Attempt login deny - {packet.DenyReason} {(packet.Custom.IsPresent ? $"({packet.Custom.Value})" : "")}", LoggingTarget.Network);
    }
}
