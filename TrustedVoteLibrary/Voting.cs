using System.Security.Cryptography;
using System.Text;

namespace TrustedVoteLibrary;

public class Voting
{
    public static byte[] SignVote(string vote, RSA rsa)
    {
        byte[] voteBytes = Encoding.UTF8.GetBytes(vote);
        return rsa.SignData(voteBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}
