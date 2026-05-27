using System.Security.Cryptography;
using ExonBreak.Game.Extensions;
using ExonBreak.Protocol;
using ExonBreak.Protocol.Packets.Login;
using osu.Framework.Logging;

namespace ExonBreak.Game.Protocol.Handlers;

public sealed class ClientboundLoginHandlers
{
    public static void HandleEncryptionRequest(ClientboundEncryptionRequestPacket packet, PacketContext context)
    {
        var sharedSecret = RandomNumberGenerator.GetBytes(16);
        Logger.Log($"Received encryption request, shared secret = {sharedSecret}", LoggingTarget.Network);

        var serverRsa = RSA.Create();
        serverRsa.ImportSubjectPublicKeyInfo(packet.ServerPublicKey, out _);
        var connection = context.PlayerConnection.AsClient();

        context.SendPacket(new ServerboundEncryptionResponsePacket(
            connection.PlayerIdentity.PublicKeyBytes,
            connection.PlayerIdentity.SignChallenge(packet.Challenge),
            serverRsa.Encrypt(sharedSecret, RSAEncryptionPadding.Pkcs1)
        ));
    }

    public static void HandleEncryptionSuccess(ClientboundEncryptionSuccessPacket packet, PacketContext context)
    {
        Logger.Log($"Encryption success", LoggingTarget.Network);
    }

}
