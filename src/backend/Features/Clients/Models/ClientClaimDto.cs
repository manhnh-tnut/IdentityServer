using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.Clients.Models;

public class ClientClaimDto
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    public string Value { get; set; }
}