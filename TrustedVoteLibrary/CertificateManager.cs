using System.Security.Cryptography;

namespace TrustedVoteLibrary;

using System;
using System.Security.Cryptography.X509Certificates;

public class CertificateManager
{
    public static X509Certificate2 GenerateSelfSignedCertificate(string subjectName, RSA rsa)
    {
        var req = new CertificateRequest(
            new X500DistinguishedName($"CN={subjectName}"), rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));
        return cert;
    }
}
