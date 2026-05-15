using System.Text.RegularExpressions;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Server.Config;

namespace ExonBreak.Server.Protocol.Handlers;

public sealed class ServerHandshakeHandlers
{
    private static readonly ClientboundDenyLoginPacket DenyVersionMismatch = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.ProtocolVersionMismatch);
    private static readonly ClientboundDenyLoginPacket DenyBanned = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.Banned);
    private static readonly ClientboundDenyLoginPacket DenyWhitelist = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.NotWhitelist);
    private static readonly ClientboundDenyLoginPacket DenyInvalidUsername = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.InvalidName);
    private static readonly ClientboundDenyLoginPacket DenyMaxPlayersOnline = new ClientboundDenyLoginPacket(ClientboundDenyLoginPacket.Reason.MaxPlayersOnline);

    private static readonly Regex UsernameRegex = new Regex("^[a-zA-Z0-9_]{3,16}$", RegexOptions.Compiled);

    public static void HandleHandshake(ServerboundHandshakeRequestPacket packet, PacketContext context)
    {
        context.SendPacket(new ClientboundHandshakeResponsePacket(SharedConstants.PROTOCOL_VERSION, DedicatedServer.CachedStatus));
    }

    public static void HandleLoginAttempt(ServerboundAttemptLoginPacket packet, PacketContext context)
    {
        var player = packet.PlayerInfo;

        if (player.ClientProtocolVersion != SharedConstants.PROTOCOL_VERSION)
        {
            context.SendPacket(DenyVersionMismatch);
            return;
        }

        if (ServerConfig.BannedPlayers.Contains(player.Id))
        {
            context.SendPacket(DenyBanned);
            return;
        }

        if (ServerConfig.WhitelistEnabled && !ServerConfig.WhitelistedPlayers.Contains(player.Id))
        {
            context.SendPacket(DenyWhitelist);
            return;
        }

        if (!UsernameRegex.IsMatch(player.Username))
        {
            context.SendPacket(DenyInvalidUsername);
            return;
        }

        if (DedicatedServer.OnlinePlayers >= DedicatedServer.MaxPlayers)
        {
            context.SendPacket(DenyMaxPlayersOnline);
            return;
        }

        context.SendPacket(new ClientboundAcceptLoginPacket());
    }
}
