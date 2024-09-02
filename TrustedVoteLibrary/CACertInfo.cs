namespace TrustedVoteLibrary;

public class CACertInfo
{
    public string CACertFileName { get; set; } // CA certificate file name
    public string CACertPath { get; set; } // CA certificate path
    public string? City { get; set; }
    public string CommonName { get; set; } // common name
    public string Country { get; set; }
    public string County { get; set; }
    public string Email { get; set; }  // issuers email
    public string FriendlyName { get; set; } // friendly name
    public string Id { get; set; } // unique identifier
    public string Issuer { get; set; } // issuer name
    public string Organization { get; set; }
    public string OrganizationalUnit { get; set; }
    public string Password { get; set; } // password
    public string State { get; set; }
    public string? ValidFrom { get; set; } // certificate valid from
    public string? ValidTo { get; set; } // certificate valid to
    // public CngKeyUsages KeyUsage { get; set; } // key usage



    public string GenerateSubject()
    {
        var subjectBuilder = new StringBuilder();

        if (!string.IsNullOrEmpty(Country))
            subjectBuilder.Append($"C={Country}, ");
        if (!string.IsNullOrEmpty(State))
            subjectBuilder.Append($"ST={State}, ");
        if (!string.IsNullOrEmpty(County))
            subjectBuilder.Append($"L={County}, ");
        if (!string.IsNullOrEmpty(Organization))
            subjectBuilder.Append($"O={Organization}, ");
        if (!string.IsNullOrEmpty(OrganizationalUnit))
            subjectBuilder.Append($"OU={OrganizationalUnit}, ");
        if (!string.IsNullOrEmpty(CommonName))
            subjectBuilder.Append($"CN={CommonName}, ");
        if (!string.IsNullOrEmpty(Email))
            subjectBuilder.Append($"E={Email}, ");

        // Remove the trailing comma and space
        if (subjectBuilder.Length > 0)
            subjectBuilder.Length -= 2;

        return subjectBuilder.ToString();
    }
}
