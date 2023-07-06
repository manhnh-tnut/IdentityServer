using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.Clients.Models;

public class ClientPropertiesDto
{
    public ClientPropertiesDto()
    {
        ClientProperties = new List<ClientPropertyDto>();
    }

    public int ClientPropertyId { get; set; }

    public int ClientId { get; set; }

    public string ClientName { get; set; }

    [Required]
    public string Key { get; set; }

    [Required]
    public string Value { get; set; }

    public List<ClientPropertyDto> ClientProperties { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}