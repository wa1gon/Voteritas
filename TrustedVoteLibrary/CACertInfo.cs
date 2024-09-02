namespace TrustedVoteLibrary;

public class CACertInfo
{
    public string Country { get; set; }
    public string State { get; set; }
    public string County { get; set; }
    public string Organization { get; set; }
    public string OrganizationalUnit { get; set; }
    public string? City { get; set; }
    public string Id { get; set; } // unique identifier
    public string Email { get; set; }  // issuers email
    public string CommonName { get; set; } // common name
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
