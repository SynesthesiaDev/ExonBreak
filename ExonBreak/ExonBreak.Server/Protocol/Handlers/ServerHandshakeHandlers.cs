using System.Security.Cryptography;
using System.Text.RegularExpressions;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Packets.Login;
using ExonBreak.Server.Config;
using ExonBreak.Server.Extensions;
using Serilog;

namespace ExonBreak.Server.Protocol.Handlers;

public sealed class ServerHandshakeHandlers
{
    private static readonly ClientboundDisconnectPacket deny_version_mismatch = new ClientboundDisconnectPacket(ClientboundDisconnectPacket.Reason.ProtocolVersionMismatch);
    private static readonly ClientboundDisconnectPacket deny_banned = new ClientboundDisconnectPacket(ClientboundDisconnectPacket.Reason.Banned);
    private static readonly ClientboundDisconnectPacket deny_whitelist = new ClientboundDisconnectPacket(ClientboundDisconnectPacket.Reason.NotWhitelist);
    private static readonly ClientboundDisconnectPacket deny_invalid_username = new ClientboundDisconnectPacket(ClientboundDisconnectPacket.Reason.InvalidName);
    private static readonly ClientboundDisconnectPacket deny_max_players_online = new ClientboundDisconnectPacket(ClientboundDisconnectPacket.Reason.MaxPlayersOnline);

    private static readonly Regex username_regex = new Regex("^[a-zA-Z0-9_]{3,16}$", RegexOptions.Compiled);

    public static void HandleHandshake(ServerboundHandshakeRequestPacket packet, PacketContext context)
    {
        Log.Information("Received handshake from player {client}", packet.PlayerInfo);
        context.SendPacket(new ClientboundHandshakeResponsePacket(SharedConstants.PROTOCOL_VERSION, DedicatedServer.CachedStatus));
        context.PlayerConnection.AsServer().PlayerInfo = packet.PlayerInfo;
    }

    public static void HandleLoginAttempt(ServerboundAttemptLoginPacket packet, PacketContext context)
    {
        var player = packet.PlayerInfo;

        if (player.ClientProtocolVersion != SharedConstants.PROTOCOL_VERSION)
        {
            context.SendPacket(deny_version_mismatch);
            return;
        }

        if (ServerConfig.BannedPlayers.Contains(player.Id))
        {
            context.SendPacket(deny_banned);
            return;
        }

        if (ServerConfig.WhitelistEnabled && !ServerConfig.WhitelistedPlayers.Contains(player.Id))
        {
            context.SendPacket(deny_whitelist);
            return;
        }

        if (!username_regex.IsMatch(player.Username))
        {
            context.SendPacket(deny_invalid_username);
            return;
        }

        if (DedicatedServer.OnlinePlayers >= ServerConfig.MaxPlayers)
        {
            context.SendPacket(deny_max_players_online);
            return;
        }

        context.SendPacket(new ClientboundAcceptLoginPacket());

        var challenge = RandomNumberGenerator.GetBytes(32);
        var connection = context.PlayerConnection.AsServer();
        connection.PendingChallengeBytes = challenge;

        context.SendPacket(new ClientboundEncryptionRequestPacket(DedicatedServer.PublicKey, challenge));

        //Tomorrow todo
        // - finish encryption networking
        // - game storage
        // - tile mapssssssssss rendering
    }
}
