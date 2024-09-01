namespace TrustedVoteLibrary;

public class VotingArea
{
    public const string Unincorporated = "Unincorporated";
    public string Country { get; set; }
    public string State { get; set; }
    public string County { get; set; }
    public string? City { get; set; }
    public string District { get; set; }
    public string Precinct { get; set; } 
    public string Id { get; set; } // unique identifier
    public string Email { get; set; }  // issuers email
    public string Ward { get; set; } // ward number

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
        return $"C={Country}, ST={State}, L={BuildLocality()}, O={District}, OU={Precinct}, CN={Id}, E={Email}";
    }
}
