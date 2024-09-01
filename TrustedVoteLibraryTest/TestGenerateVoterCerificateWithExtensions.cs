using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using TrustedVoteLibrary;
using TrustedVoteLibrary.Utils;

namespace TestProject1
{
    [TestClass]
    public class CertificateManagerTest
    {
        [TestMethod]
        public void TestGenerateVoterCertificateWithExtensions()
        {
            // Arrange

            string guid = Guid.NewGuid().ToString();


            using (RSA rsa = RSA.Create())
            {
                var voteArea = new VotingArea()
                {
                    Country = "US",
                    State = "AR",
                    County = "Benton",
                    City = "Rogers",
                    Ward = "1",
                    Precinct = "2",
                    District = "42",
                    Id = guid,
                    Email = "clerk@anytown.St.US"
                };
                // Act
                var cert = CertificateManager.GenerateVoterCertificateWithExtensions(
                    voteArea, rsa);

                // Assert
                Assert.IsNotNull(cert, "Certificate should not be null");
                Assert.AreEqual(cert.GetSubjectValueByName("CN"), guid, "Common Name above guid");
                Assert.AreEqual(cert.GetSubjectValueByName("E"), voteArea.Email, "Email should match"); }
        }
    }
}
