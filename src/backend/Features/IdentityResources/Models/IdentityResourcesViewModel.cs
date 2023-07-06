namespace IdentityServer.Features.IdentityResources.Models;

public class IdentityResourcesViewModel
{
    public IdentityResourcesViewModel()
    {
        IdentityResources = new List<IdentityResourceViewModel>();
    }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public List<IdentityResourceViewModel> IdentityResources { get; set; }
}