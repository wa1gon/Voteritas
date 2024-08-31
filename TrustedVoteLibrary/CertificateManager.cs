
public class CertificateManager
{

        public static X509Certificate2 GenerateVoterCertificateWithExtensions(string stateAbbreviation, 
            string votingDistrict, 
            string guid, 
            string issuerId, 
            RSA rsa)
        {
            // Start with the most basic and known good format
            var subjectName = $"CN=Voter, C={stateAbbreviation}";

            // Add more components cautiously
            // Notice that we avoid using UID or other uncommon attributes
            // The issuerId is now placed in a custom extension instead
            var dn = new X500DistinguishedName(subjectName);

            // Create the certificate request
            var req = new CertificateRequest(dn, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            // Add custom extensions if needed
            var issuerIdExtension = new X509Extension(new Oid("1.2.3.4.5.4", "Issuer ID"), 
                Encoding.ASCII.GetBytes(issuerId), false);
            req.CertificateExtensions.Add(issuerIdExtension);

            // Create the self-signed certificate
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(1));

            return cert;
        }
    }
