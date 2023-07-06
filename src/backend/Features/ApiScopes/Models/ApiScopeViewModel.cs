using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.ApiScopes.Models;

public class ApiScopeViewModel
{
    public ApiScopeViewModel()
    {
        UserClaims = new List<string>();
    }

    public bool ShowInDiscoveryDocument { get; set; } = true;

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool Required { get; set; }

    public bool Emphasize { get; set; }

    public List<string> UserClaims { get; set; }

    public string UserClaimsItems { get; set; }

    public bool Enabled { get; set; } = true;

    public List<ApiScopePropertyViewModel> ApiScopeProperties { get; set; }
}