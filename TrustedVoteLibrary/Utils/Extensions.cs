namespace TrustedVoteLibrary.Utils;

public static  class ExtensionsMethods
{
    public static byte[] ToBytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    public static string ToString(this byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

 
    public static string? GetSubjectValueByName(this X509Certificate2 cert, string propertyName)
    {
        var subject = cert.Subject;
        var properties = subject.Split(',');

        foreach (var property in properties)
        {
            var keyValue = property.Split('=');
            if (keyValue.Length == 2 && keyValue[0].Trim() == propertyName)
            {
                return keyValue[1].Trim();
            }
        }

        return null;
    }
}
