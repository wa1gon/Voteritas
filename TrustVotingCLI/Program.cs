using System;
using System.Security.Cryptography.X509Certificates;
using TrustedVoteLibrary;

class Program
{
    static void Main()
    {
        // Voter's Information
        string stateAbbreviation = "AR";  // Example: Arkansas
        string votingDistrict = "District 42";
        string guid = Guid.NewGuid().ToString();
        string issuerId = "Issuer123";

        // Generate RSA Key Pair
        var (rsa, publicKey, privateKey) = PKIManager.GenerateKeys();

        // Generate the Voter Certificate with the RSA key and custom extensions
        var voterCertificate = CertificateManager.GenerateVoterCertificateWithExtensions(
            stateAbbreviation, votingDistrict, guid, issuerId, rsa);

        // Display Certificate Information
        Console.WriteLine("Voter Certificate:");
        Console.WriteLine($"Subject: {voterCertificate.Subject}");
        Console.WriteLine($"Issuer: {voterCertificate.Issuer}");
        Console.WriteLine($"Serial Number: {voterCertificate.SerialNumber}");
        Console.WriteLine($"Thumbprint: {voterCertificate.Thumbprint}");
        Console.WriteLine($"Public Key: {publicKey}");

        // Optionally, save the certificate to a file
        byte[] certData = voterCertificate.Export(X509ContentType.Pfx);
        System.IO.File.WriteAllBytes("voter_cert_with_issuer.pfx", certData);

        // Dispose of the RSA object
        rsa.Dispose();
    }
}
