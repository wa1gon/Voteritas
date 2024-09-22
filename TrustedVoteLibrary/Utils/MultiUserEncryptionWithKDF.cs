namespace TrustedVoteLibrary.Utils;
using System.Numerics;

public class MultiUserEncryptionWithKDF
{
    private BigInteger user1Secret;
    private BigInteger user2Secret;
    private byte[] salt;
    private const int Iterations = 100000; // Number of iterations for PBKDF2

    public MultiUserEncryptionWithKDF(string user1KeyFile, string user2KeyFile)
    {
        // Read secrets from file and parse them as BigIntegers
        user1Secret = ReadKeyFromFile(user1KeyFile);
        user2Secret = ReadKeyFromFile(user2KeyFile);

        salt = GenerateRandomSalt(); // Generate a random salt for KDF
    }

    // Read the key from a file
    private BigInteger ReadKeyFromFile(string filePath)
    {
        try
        {
            // Read the key as a string and convert it to BigInteger
            string keyText = File.ReadAllText(filePath);
            return BigInteger.Parse(keyText.Trim()); // Trim to remove any extra spaces or newlines
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Error reading key from file '{filePath}': {ex.Message}");
        }
    }

    // Method to combine both users' keys securely using PBKDF2
    private byte[] CombineKeys()
    {
        // Convert BigIntegers to byte arrays
        byte[] secret1Bytes = user1Secret.ToByteArray();
        byte[] secret2Bytes = user2Secret.ToByteArray();

        // Combine both keys into a single byte array (concatenation)
        byte[] combinedSecret = new byte[secret1Bytes.Length + secret2Bytes.Length];
        Buffer.BlockCopy(secret1Bytes, 0, combinedSecret, 0, secret1Bytes.Length);
        Buffer.BlockCopy(secret2Bytes, 0, combinedSecret, secret1Bytes.Length, secret2Bytes.Length);

        // Derive a key using PBKDF2
        using var kdf = new Rfc2898DeriveBytes(combinedSecret, salt, Iterations, HashAlgorithmName.SHA256);
        return kdf.GetBytes(32); // Derive a 32-byte key for AES-256
    }

    // Method to encrypt a message using the derived key
    public byte[] EncryptMessage(string plaintext)
    {
        byte[] derivedKey = CombineKeys();
        using Aes aes = Aes.Create();
        aes.Key = derivedKey;
        aes.GenerateIV();

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
        byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // Return the encrypted message along with IV
        byte[] result = new byte[aes.IV.Length + encrypted.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

        return result;
    }

    // Method to decrypt the message using both users' keys
    public string DecryptMessage(byte[] ciphertext)
    {
        byte[] derivedKey = CombineKeys();
        using (Aes aes = Aes.Create())
        {
            aes.Key = derivedKey;

            // Extract the IV from the ciphertext
            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] actualCiphertext = new byte[ciphertext.Length - iv.Length];

            Buffer.BlockCopy(ciphertext, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(ciphertext, iv.Length, actualCiphertext, 0, actualCiphertext.Length);

            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] decryptedBytes = decryptor.TransformFinalBlock(actualCiphertext, 0, actualCiphertext.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    // Generate a random salt for the KDF
    private byte[] GenerateRandomSalt()
    {
        byte[] salt = new byte[16]; // 16-byte salt
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}
