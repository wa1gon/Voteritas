namespace TrustedVoteLibrary;

public class VoteEncryption
{
    public static byte[] EncryptVote(string vote, RSA authorityRsa)
    {
        byte[] voteBytes = Encoding.UTF8.GetBytes(vote);
        return authorityRsa.Encrypt(voteBytes, RSAEncryptionPadding.OaepSHA256);
    }
}
