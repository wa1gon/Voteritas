using System.Security.Cryptography;

namespace TestProject1;
[TestClass]
public class CertificateManagerTest
{
    [TestMethod]
    public void TestGenerateVoterCertificateWithExtensions()
    {
        // Arrange
        string stateAbbreviation = "CA";
        string votingDistrict = "District1";
        string guid = "some-guid";
        string issuerId = "issuer-id";

        using (RSA rsa = RSA.Create())
        {
            // Act
            var cert = CertificateManager.GenerateVoterCertificateWithExtensions(
                stateAbbreviation, votingDistrict, guid, issuerId, rsa);

            // Assert
            Assert.IsNotNull(cert, "Certificate should not be null");
            Assert.AreEqual($"CN=Voter, C={stateAbbreviation}", cert.Subject, "Certificate subject should match");
            Assert.IsTrue(cert.Extensions.Count > 0, "Certificate should have extensions");
        }
    }
}
