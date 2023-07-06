using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.ApiResources.Models;

public class ApiResourceViewModel
{
    public ApiResourceViewModel()
    {
        UserClaims = new List<string>();
        Scopes = new List<string>();
        AllowedAccessTokenSigningAlgorithms = new List<string>();
    }

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; } = true;

    public bool ShowInDiscoveryDocument { get; set; }

    public bool RequireResourceIndicator { get; set; }

    public List<string> UserClaims { get; set; }

    public List<string> AllowedAccessTokenSigningAlgorithms { get; set; }

    public List<string> Scopes { get; set; }
}