namespace IdentityServer.Features.Clients.Models;

public class ClientClaimsViewModel
{
    public ClientClaimsViewModel()
    {
        ClientClaims = new List<ClientClaimViewModel>();
    }

    public List<ClientClaimViewModel> ClientClaims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}