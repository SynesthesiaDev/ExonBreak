using System.Security.Cryptography;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets.Handshake;
using ExonBreak.Protocol.Packets.Login;
using ExonBreak.Protocol.Types.Player;
using ExonBreak.Server.Extensions;
using ExonBreak.Server.Utils;
using Serilog;

namespace ExonBreak.Server.Protocol.Handlers;

public sealed class ServerLoginHandler
{
    public static void HandleEncryptionResponse(ServerboundEncryptionResponsePacket packet, PacketContext context)
    {
        Log.Information("Received encryption response from client");
        var connection = context.PlayerConnection.AsServer();

        if (connection.PendingChallengeBytes == null || !CryptoUtils.VerifyChallenge(packet.ClientPublicKey, connection.PendingChallengeBytes, packet.Signature))
        {
            context.Disconnect(ClientboundDisconnectPacket.Reason.EncryptionError);
            return;
        }

        var sharedSecret = DedicatedServer.CIPHER.Decrypt(packet.SharedSecret, RSAEncryptionPadding.Pkcs1);
        var guid = PlayerInfo.DeriveGuid(packet.ClientPublicKey);

        if (connection.PlayerInfo.Id != guid)
        {
            Log.Error("Identity Mismatch. Expected {guid} but actual is {real}", connection.PlayerInfo.Id, guid);
            context.Disconnect(ClientboundDisconnectPacket.Reason.IdentityMismatch);
            return;
        }

        //todo enable encryption
        context.SendPacket(new ClientboundEncryptionSuccessPacket());
    }
}
