namespace IdentityServer.Features.Clients.Models;

public class ClientPropertiesViewModel
{
    public ClientPropertiesViewModel()
    {
        ClientProperties = new List<ClientPropertyViewModel>();
    }

    public List<ClientPropertyViewModel> ClientProperties { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}