namespace TrustedVoteLibrary;

public class VoteCounting
{
    public static string DecryptVote(byte[] encryptedVote, RSA authorityRsa)
    {
        byte[] decryptedBytes = authorityRsa.Decrypt(encryptedVote, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
