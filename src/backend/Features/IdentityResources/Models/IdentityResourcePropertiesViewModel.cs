namespace IdentityServer.Features.IdentityResources.Models;

public class IdentityResourcePropertiesViewModel
{
    public IdentityResourcePropertiesViewModel()
    {
        IdentityResourceProperties = new List<IdentityResourcePropertyViewModel>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<IdentityResourcePropertyViewModel> IdentityResourceProperties { get; set; }
}