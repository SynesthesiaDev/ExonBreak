using System;
using System.Security.Cryptography;
using ExonBreak.Protocol.Types.Player;
using osu.Framework.Extensions;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace ExonBreak.Game.Persistent;

public class PlayerIdentity
{
    private const string key_file_name = "DO_NOT_SHARE-AUTH_KEY.key";

    public required RSA Cipher { get; init; }
    public required Guid Guid { get; init; }
    public required byte[] PublicKeyBytes { get; init; }

    private PlayerIdentity() { }

    public static PlayerIdentity LoadOrCreate(Storage storage)
    {
        RSA cipher;
        if (storage.Exists(key_file_name))
        {
            cipher = RSA.Create();
            var stream = storage.GetStream(key_file_name);
            var data = stream.ReadAllBytesToArray();
            stream.Close();

            cipher.ImportRSAPrivateKey(data, out _);
        }
        else
        {
            cipher = RSA.Create(2048);
            var writeStream = storage.CreateFileSafely(key_file_name);
            writeStream.Write(cipher.ExportRSAPrivateKey());
            writeStream.Close();
        }

        var pubKey = cipher.ExportSubjectPublicKeyInfo();

        var identity = new PlayerIdentity
        {
            Cipher = cipher,
            PublicKeyBytes = pubKey,
            Guid = PlayerInfo.DeriveGuid(pubKey)
        };

        Logger.Log($"Loaded Player Identity: {identity.Guid}");

        return identity;
    }

    public byte[] SignChallenge(byte[] challenge) => Cipher.SignData(challenge, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
}
