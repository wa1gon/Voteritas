using System.Security.Cryptography;
using System.Text;

namespace TrustedVoteLibrary;

public class VoteVerification
{
    public static bool VerifyVote(string vote, byte[] signature, RSA rsa)
    {
        byte[] voteBytes = Encoding.UTF8.GetBytes(vote);
        return rsa.VerifyData(voteBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}
