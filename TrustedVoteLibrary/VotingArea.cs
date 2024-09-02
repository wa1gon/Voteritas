using System.ComponentModel.DataAnnotations;

namespace TrustedVoteLibrary;

public class VotingArea
{
    public const string Unincorporated = "Unincorporated";
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string County { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Precinct { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty; // unique identifier
    public string Email { get; set; } = string.Empty; // issuers email
    public string Ward { get; set; } = string.Empty; // ward number

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
