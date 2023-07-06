namespace IdentityServer.Features.ApiScopes.Models;

public class ApiScopesViewModel
{
    public ApiScopesViewModel()
    {
        Scopes = new List<ApiScopeViewModel>();
    }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public List<ApiScopeViewModel> Scopes { get; set; }
}