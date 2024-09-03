namespace TrustedVoteLibrary;

public class VoterCert
{
    public string CommonName { get; set; } = string.Empty;
    public const string Unincorporated = "Unincorporated";
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string County { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Precinct { get; set; } = string.Empty;
    public string Ward { get; set; } = "none"; // ward number
    public string Id { get; set; } = string.Empty; // unique identifier
    public string Email { get; set; } = string.Empty; // voter email
    public string Organization { get; set; } = string.Empty; // voter party  parties are D, R, I, L ,G, etc.
    public string OrganizationalUnit { get; set; } // normally ward
    public string Party { get; set; } = string.Empty; // voter party  parties are D, R, I, L ,G, etc.

    public string BuildLocality()
    {
        var city = City ?? Unincorporated;
        return $"{County}:{city}:{Ward}";
    }

    public void SetLocality(string? locality)
    {
        if (locality == null)
            return;

        var parts = locality.Split(':');
        County = parts[0];
        City = parts[1];
        Ward = parts[2];
    }

    public string GenerateSubject()
    {
        var subjectBuilder = new StringBuilder();
        if (CommonName.IsNullOrEmpty()) throw new NullReferenceException("Name is required");
        if (Organization.IsNullOrEmpty()) throw new NullReferenceException("Organization AKA Party is required");
        if (OrganizationalUnit.IsNullOrEmpty()) throw new NullReferenceException("OrganizationalUnit is required");
        if (Country.IsNullOrEmpty()) throw new NullReferenceException("Country is required");
        if (State.IsNullOrEmpty()) throw new NullReferenceException("State is required");
        if (County.IsNullOrEmpty()) throw new NullReferenceException("County is required");
        if (Id.IsNullOrEmpty()) throw new NullReferenceException("Id is required");

        subjectBuilder.Append($"CN={CommonName}, ");
        subjectBuilder.Append($"O={Organization}, ");
        if (!string.IsNullOrEmpty(OrganizationalUnit))
            subjectBuilder.Append($"OU={OrganizationalUnit}, ");     
        
        subjectBuilder.Append($"L={BuildLocality()}, ");
        subjectBuilder.Append($"ST={State}, ");
        subjectBuilder.Append($"C={Country}, ");
        if (!string.IsNullOrEmpty(Email))
            subjectBuilder.Append($"EMAIL={Email}, ");
        subjectBuilder.Append($"UID={Id}, ");

        
        // Remove the trailing comma and space
        if (subjectBuilder.Length > 0)
            subjectBuilder.Length -= 2;

        return subjectBuilder.ToString();
    }
}
