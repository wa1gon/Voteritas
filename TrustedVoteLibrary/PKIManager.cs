namespace TrustedVoteLibrary;

using System;
using System.Security.Cryptography;

public class PKIManager
{
    public static (string publicKey, string privateKey) GenerateKeys()
    {
        using (var rsa = RSA.Create())
        {
            return (Convert.ToBase64String(rsa.ExportRSAPublicKey()), 
                Convert.ToBase64String(rsa.ExportRSAPrivateKey()));
        }
    }
}
