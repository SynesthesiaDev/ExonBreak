using System.Text.RegularExpressions;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Server.Config;

namespace ExonBreak.Server.Protocol.Handlers;

public sealed class HandshakeHandlers
{
    private static readonly ClientboundDenyLoginPacket deny_version_mismatch = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.ProtocolVersionMismatch);
    private static readonly ClientboundDenyLoginPacket deny_banned = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.Banned);
    private static readonly ClientboundDenyLoginPacket deny_whitelist = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.NotWhitelist);
    private static readonly ClientboundDenyLoginPacket deny_invalid_username = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.InvalidName);
    private static readonly ClientboundDenyLoginPacket deny_max_players_online = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.MaxPlayersOnline);

    private static readonly Regex username_regex = new Regex("^[a-zA-Z0-9_]{3,16}$", RegexOptions.Compiled);

    public static void HandleHandshake(ServerboundHandshakeRequestPacket packet, PacketContext context)
    {
        context.SendPacket(new ClientboundHandshakeResponsePacket(SharedConstants.PROTOCOL_VERSION, DedicatedServer.CachedStatus));
    }

    public static void HandleLoginAttempt(ServerboundAttemptLoginPacket packet, PacketContext context)
    {
        var player = packet.PlayerInfo;

        if (player.ClientProtocolVersion != SharedConstants.PROTOCOL_VERSION)
        {
            context.SendPacket(deny_version_mismatch);
            return;
        }

        // if (DedicatedServer.BannedPlayers.Contains(player.Id))
        // {
        //     context.SendPacket(deny_banned);
        //     return;
        // }

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

        if (DedicatedServer.OnlinePlayers >= DedicatedServer.MaxPlayers)
        {
            context.SendPacket(deny_max_players_online);
            return;
        }

        context.SendPacket(new ClientboundAcceptLoginPacket());
    }
}
