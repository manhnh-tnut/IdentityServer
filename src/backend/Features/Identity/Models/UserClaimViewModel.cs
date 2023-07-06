using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.Identity.Models;

public class UserClaimViewModel<TKey>
{
    public int ClaimId { get; set; }

    public TKey UserId { get; set; }

    [Required]
    public string ClaimType { get; set; }

    [Required]
    public string ClaimValue { get; set; }
}