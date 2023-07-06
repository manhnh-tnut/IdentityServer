namespace IdentityServer.Features.ApiResources.Models;

public class ApiResourcesViewModel
{
    public ApiResourcesViewModel()
    {
        ApiResources = new List<ApiResourceViewModel>();
    }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public List<ApiResourceViewModel> ApiResources { get; set; }
}