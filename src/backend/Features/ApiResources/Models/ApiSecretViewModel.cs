using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.ApiResources.Models;

public class ApiSecretViewModel
{
    [Required]
    public string Type { get; set; } = "SharedSecret";

    public int Id { get; set; }

    public string Description { get; set; }

    [Required]
    public string Value { get; set; }

    public string HashType { get; set; }

    public DateTime? Expiration { get; set; }
}