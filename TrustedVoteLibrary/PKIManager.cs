namespace TrustedVoteLibrary;

public class PKIManager
{
    public static (RSA rsa, string publicKey, string privateKey) GenerateKeys()
    {
        using RSA rsa = RSA.Create(2048);
        string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
        string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
        return (rsa, publicKey, privateKey);
    }
}
