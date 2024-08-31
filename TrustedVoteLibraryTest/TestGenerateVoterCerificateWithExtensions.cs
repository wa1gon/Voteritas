using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TrustedVoteLibrary;

namespace TestProject1
{
    [TestClass]
    public class CertificateManagerTest
    {
        [TestMethod]
        public void TestGenerateVoterCertificateWithExtensions()
        {
            // Arrange
            string stateAbbreviation = "CA";
            string votingDistrict = "San Francisco";
            string guid = Guid.NewGuid().ToString();


            using (RSA rsa = RSA.Create())
            {
                // Act
                var cert = CertificateManager.GenerateVoterCertificateWithExtensions(
                    stateAbbreviation, votingDistrict, guid, rsa);

                // Assert
                Assert.IsNotNull(cert, "Certificate should not be null");
                Assert.AreEqual($"CN=Voter, C={stateAbbreviation}, L={votingDistrict}", cert.Subject, "Certificate subject should match");
                Assert.IsTrue(cert.Extensions.Count > 0, "Certificate should have extensions");
                Assert.AreEqual("CN=Voter, C=CA, L=San Francisco",cert.Subject, "Certificate subject should match");
                // Verify the voting district is correct
                var votingDistrictExtension = cert.Extensions["1.2.3.4.5.6.7.8.1"];
                Assert.IsNotNull(votingDistrictExtension, "Voting district extension should not be null");
                Assert.AreEqual(votingDistrict, System.Text.Encoding.ASCII.GetString(votingDistrictExtension.RawData), "Voting district should match");
            }
        }
    }
}
