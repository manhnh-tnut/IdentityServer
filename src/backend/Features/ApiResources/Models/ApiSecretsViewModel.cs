namespace IdentityServer.Features.ApiResources.Models;

public class ApiSecretsViewModel
{
    public ApiSecretsViewModel()
    {
        ApiSecrets = new List<ApiSecretViewModel>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<ApiSecretViewModel> ApiSecrets { get; set; }
}