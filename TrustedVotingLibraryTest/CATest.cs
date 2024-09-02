namespace TrustedVotingLibraryTest;

[TestClass]
public class CATest
{
    [TestMethod]
    public void TestGenerateVoterCertificateWithExtensions()
    {
        // Arrange

        string guid = Guid.NewGuid().ToString();


        using (RSA rsa = RSA.Create())
        {
            var caInfo = new CACertInfo()
            {
                Country = "US",
                State = "AR",
                County = "Benton",
                City = "Rogers",
                CommonName = "OpenVoting",
                Id = guid,
                Email = "clerk@anytown.St.US"
            };
            // Act

            X509Certificate2 caCert = CertificateAuthority.GenerateCACertificate(caInfo);

            // Assert
            Assert.IsNotNull(caCert, "Certificate should not be null");
            var cn = caCert.GetSubjectValueByName("CN");
            Assert.AreEqual(cn, caInfo.CommonName, "Common Name ");

            CertificateAuthority.SaveCertificateWithPrivateKey(caCert,"/tmp/testCA.pfx","kb1etc");
        }
    }
}
