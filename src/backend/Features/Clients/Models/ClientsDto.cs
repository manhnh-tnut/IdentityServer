namespace IdentityServer.Features.Clients.Models;

public class ClientsDto
{
    public ClientsDto()
    {
        Clients = new List<ClientDto>();
    }

    public List<ClientDto> Clients { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}