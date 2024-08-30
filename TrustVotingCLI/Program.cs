// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using TrustedVoteLibrary;

class Program
{
    static void Main()
    {
        // Setup PKI for the voting authority
        var authorityRsa = RSA.Create();
        var authorityCert = CertificateManager.GenerateSelfSignedCertificate("Voting Authority", authorityRsa);

        // Voter Registration
        var voterRsa = RSA.Create();
        var voterCert = CertificateManager.GenerateSelfSignedCertificate("Voter 1", voterRsa);

        var registration = new VoterRegistration();
        registration.RegisterVoter("Voter1", voterCert);

        // Voting
        string vote = "Candidate A";
        var signedVote = Voting.SignVote(vote, voterRsa);
        var encryptedVote = VoteEncryption.EncryptVote(vote, authorityRsa);

        // Vote Verification
        var verified = VoteVerification.VerifyVote(vote, signedVote, voterRsa);
        Console.WriteLine("Vote verification: " + (verified ? "Success" : "Failure"));

        // Vote Counting
        var decryptedVote = VoteCounting.DecryptVote(encryptedVote, authorityRsa);
        Console.WriteLine("Decrypted vote: " + decryptedVote);
    }
}
