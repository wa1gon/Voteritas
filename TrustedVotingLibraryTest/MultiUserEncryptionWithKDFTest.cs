namespace TrustedVotingLibraryTest;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Numerics;

[TestClass]
public class MultiUserEncryptionWithKDFUnitTests
{
    private string user1KeyFile;
    private string user2KeyFile;

    // Setup method to create temporary key files before each test
    [TestInitialize]
    public void SetUp()
    {
        // Create temporary user key files with valid BigInteger values
        user1KeyFile = Path.GetTempFileName();
        user2KeyFile = Path.GetTempFileName();

        File.WriteAllText(user1KeyFile, "340282366920938463463374607431768211456"); // Example 128-bit key
        File.WriteAllText(user2KeyFile, "170141183460469231731687303715884105728"); // Example 128-bit key
    }

    // Cleanup method to delete temporary files after each test
    [TestCleanup]
    public void CleanUp()
    {
        File.Delete(user1KeyFile);
        File.Delete(user2KeyFile);
    }

    // Happy Path: Test encryption and decryption with valid key files
    [TestMethod]
    public void TestEncryptDecrypt_ValidKeys_ShouldWork()
    {
        // Arrange
        MultiUserEncryptionWithKDF multiUserEnc = new MultiUserEncryptionWithKDF(user1KeyFile, user2KeyFile);
        string message = "This is a test message.";

        // Act
        byte[] encryptedMessage = multiUserEnc.EncryptMessage(message);
        string decryptedMessage = multiUserEnc.DecryptMessage(encryptedMessage);

        // Assert
        Assert.AreEqual(message, decryptedMessage);
    }

    // Sad Path: Test with non-existing key file
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestKeyFileNotFound_ShouldThrowException()
    {
        // Arrange
        string invalidKeyFile = "non_existing_file.key"; // This file does not exist

        // Act
        MultiUserEncryptionWithKDF multiUserEnc = new MultiUserEncryptionWithKDF(invalidKeyFile, user2KeyFile);
    }

    // Sad Path: Test with invalid key content in file (non-numeric)
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestInvalidKeyContent_ShouldThrowException()
    {
        // Arrange: Write invalid (non-numeric) content to user1 key file
        File.WriteAllText(user1KeyFile, "invalid_key_content");

        // Act
        MultiUserEncryptionWithKDF multiUserEnc = new MultiUserEncryptionWithKDF(user1KeyFile, user2KeyFile);
    }

    // Sad Path: Test with valid file but too small key
    // [TestMethod]
    // [ExpectedException(typeof(ArgumentException))]
    // public void TestTooSmallKey_ShouldThrowException()
    // {
    //     // Arrange: Write a very small key (less than 128-bit)
    //     File.WriteAllText(user1KeyFile, "12345");
    //
    //     // Act
    //     MultiUserEncryptionWithKDF multiUserEnc = new MultiUserEncryptionWithKDF(user1KeyFile, user2KeyFile);
    // }
}
