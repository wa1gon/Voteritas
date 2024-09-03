namespace TrustedVotingLibraryTest;

[TestClass]
public class VoterCertTests
{
    [TestMethod]
    public void GenerateSubject_HappyPath_ReturnsCorrectSubject()
    {
        string guid = Guid.NewGuid().ToString();
        // Arrange
        var voterCert = new VoterCert
        {
            CommonName = "John Doe",
            Organization = "D",
            OrganizationalUnit = "Ward 1",
            Country = "US",
            State = "CA",
            County = "Los Angeles",
            City = "Los Angeles",
            Ward = "1",
            Id = guid,
            Email = "john.doe@example.com"
        };

        // Act
        var subject = voterCert.GenerateSubject();
        var expectedSubject = $"CN=John Doe, O=D, OU=Ward 1, L=Los Angeles:Los Angeles:1, ST=CA, C=US, EMAIL=john.doe@example.com, UID={guid}";
        // Assert
        Assert.AreEqual(expectedSubject, subject);
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GenerateSubject_MissingCommonName_ThrowsException()
    {
        // Arrange
        var voterCert = new VoterCert
        {
            Organization = "D",
            OrganizationalUnit = "Ward 1",
            Country = "US",
            State = "CA",
            County = "Los Angeles",
            City = "Los Angeles",
            Ward = "1",
            Id = "12345",
            Email = "john.doe@example.com"
        };

        // Act
        voterCert.GenerateSubject();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GenerateSubject_MissingOrganization_ThrowsException()
    {
        // Arrange
        var voterCert = new VoterCert
        {
            CommonName = "John Doe",
            OrganizationalUnit = "Ward 1",
            Country = "US",
            State = "CA",
            County = "Los Angeles",
            City = "Los Angeles",
            Ward = "1",
            Id = "12345",
            Email = "john.doe@example.com"
        };

        // Act
        voterCert.GenerateSubject();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GenerateSubject_MissingOrganizationalUnit_ThrowsException()
    {
        // Arrange
        var voterCert = new VoterCert
        {
            CommonName = "John Doe",
            Organization = "D",
            Country = "US",
            State = "CA",
            County = "Los Angeles",
            City = "Los Angeles",
            Ward = "1",
            Id = "12345",
            Email = "john.doe@example.com"
        };

        // Act
        voterCert.GenerateSubject();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GenerateSubject_MissingCountry_ThrowsException()
    {
        // Arrange
        var voterCert = new VoterCert
        {
            CommonName = "John Doe",
            Organization = "D",
            OrganizationalUnit = "Ward 1",
            State = "CA",
            County = "Los Angeles",
            City = "Los Angeles",
            Ward = "1",
            Id = "12345",
            Email = "john.doe@example.com"
        };

        // Act
        voterCert.GenerateSubject();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GenerateSubject_MissingState_ThrowsException()
    {
        // Arrange
        var voterCert = new VoterCert
        {
            CommonName = "John Doe",
            Organization = "D",
            OrganizationalUnit = "Ward 1",
            Country = "US",
            County = "Los Angeles",
            City = "Los Angeles",
            Ward = "1",
            Id = "12345",
            Email = "john.doe@example.com"
        };

        // Act
        voterCert.GenerateSubject();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GenerateSubject_MissingCounty_ThrowsException()
    {
        // Arrange
        var voterCert = new VoterCert
        {
            CommonName = "John Doe",
            Organization = "D",
            OrganizationalUnit = "Ward 1",
            Country = "US",
            State = "CA",
            City = "Los Angeles",
            Ward = "1",
            Id = "12345",
            Email = "john.doe@example.com"
        };

        // Act
        voterCert.GenerateSubject();
    }

    [TestMethod]
    [ExpectedException(typeof(NullReferenceException))]
    public void GenerateSubject_MissingId_ThrowsException()
    {
        // Arrange
        var voterCert = new VoterCert
        {
            CommonName = "John Doe",
            Organization = "D",
            OrganizationalUnit = "Ward 1",
            Country = "US",
            State = "CA",
            County = "Los Angeles",
            City = "Los Angeles",
            Ward = "1",
            Email = "john.doe@example.com"
        };

        // Act
        voterCert.GenerateSubject();
    }
}
