namespace IdentityServer.Features.ApiResources.Models;

public class ApiResourcePropertiesViewModel
{
    public ApiResourcePropertiesViewModel()
    {
        ApiResourceProperties = new List<ApiResourcePropertyViewModel>();
    }

    public List<ApiResourcePropertyViewModel> ApiResourceProperties { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}