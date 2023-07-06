namespace IdentityServer.Features.Clients.Models;

public class ClientSecretsViewModel
{
    public ClientSecretsViewModel()
    {
        ClientSecrets = new List<ClientSecretViewModel>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<ClientSecretViewModel> ClientSecrets { get; set; }
}