namespace TrustedVoteLibrary.Utils;

using System.Security.Cryptography;

public class KeyGenerator
{
    public static byte[] Generate128BitKey()
    {
        // 128 bits = 16 bytes
        byte[] key = new byte[16];

        // Using RandomNumberGenerator to generate a cryptographically secure random key
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);

        return key;
    }

    // Optional: Convert the key to a hex string for easier viewing
    public static string ByteArrayToHexString(byte[] byteArray)
    {
        return BitConverter.ToString(byteArray).Replace("-", "").ToLower();
    }
}
