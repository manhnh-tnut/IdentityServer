using System.ComponentModel.DataAnnotations;
using IdentityServer.Features.Shared.Models;
using IdentityServer.Infrastructure.Enums;

namespace IdentityServer.Features.Clients.Models;

public class ClientSecretsDto
{
    public ClientSecretsDto()
    {
        ClientSecrets = new List<ClientSecretDto>();
    }

    public int ClientId { get; set; }

    public string ClientName { get; set; }

    public int ClientSecretId { get; set; }

    [Required]
    public string Type { get; set; } = "SharedSecret";

    public List<SelectItemDto> TypeList { get; set; }

    public string Description { get; set; }

    [Required]
    public string Value { get; set; }

    public string HashType { get; set; }

    public HashType HashTypeEnum => Enum.TryParse(HashType, true, out HashType result) ? result : IdentityServer.Infrastructure.Enums.HashType.Sha256;

    public List<SelectItemDto> HashTypes { get; set; }

    public DateTime? Expiration { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<ClientSecretDto> ClientSecrets { get; set; }
}