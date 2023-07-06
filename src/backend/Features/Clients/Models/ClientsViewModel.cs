namespace IdentityServer.Features.Clients.Models;

public class ClientsViewModel
{
    public ClientsViewModel()
    {
        Clients = new List<ClientViewModel>();
    }

    public List<ClientViewModel> Clients { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}