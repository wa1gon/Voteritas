using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TrustedVoteLibrary;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var certPath = configuration["CertPath"];
        var certPassword = configuration["CertPassword"];
        var caCertPath = configuration["CACertPath"];
        
        
        
        // Load the CA certificate and private key
        // X509Certificate2 caCert = new X509Certificate2("MyRootCA.pfx", "YourSecurePassword");
        // RSA? caPrivateKey = caCert.GetRSAPrivateKey();
        //
        // // Subject name for the new certificate
        // string subjectName = "CN=MySignedCert, O=MyOrganization, C=US";
        //
        // // Create the certificate signed by the CA
        // if (caPrivateKey != null)
        // {
        //     X509Certificate2 signedCert = CertificateGenerator.CreateCertificate(subjectName, caCert, caPrivateKey);
        //
        //     // Save the signed certificate with the private key to a PFX file
        //     byte[] pfxData = signedCert.Export(X509ContentType.Pfx, "CertPassword");
        //     System.IO.File.WriteAllBytes("MySignedCert.pfx", pfxData);
        }

        // Console.WriteLine("Certificate created and saved successfully.");
    }
}
