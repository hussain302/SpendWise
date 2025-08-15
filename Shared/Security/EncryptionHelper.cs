using System.Security.Cryptography;
using System.Text;

namespace Shared.Security;

public static class EncryptionHelper
{
    public static string Encrypt(string textToEncrypt, string encryptionKey)
    {
        byte[] keyBytes = GetValidKey(encryptionKey);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = keyBytes;
        aesAlg.IV = new byte[aesAlg.BlockSize / 8];

        using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(textToEncrypt);
        }

        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    public static string Decrypt(string encryptedText, string encryptionKey)
    {
        byte[] keyBytes = GetValidKey(encryptionKey);
        byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = keyBytes;
        aesAlg.IV = new byte[aesAlg.BlockSize / 8];

        using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        using var msDecrypt = new MemoryStream(cipherTextBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }

    private static byte[] GetValidKey(string key)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(key)).Take(32).ToArray();
    }

}
