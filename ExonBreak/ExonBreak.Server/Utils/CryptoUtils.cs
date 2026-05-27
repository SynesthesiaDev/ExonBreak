using System.Security.Cryptography;

namespace ExonBreak.Server.Utils;

public class CryptoUtils
{
    public static bool VerifyChallenge(byte[] publicKeyBytes, byte[] challenge, byte[] signature)
    {
        using var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
        return rsa.VerifyData(challenge, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}
