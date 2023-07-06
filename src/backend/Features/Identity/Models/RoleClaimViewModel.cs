using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.Identity.Models;

public class RoleClaimViewModel<TKey>
{
    public int ClaimId { get; set; }

    public TKey RoleId { get; set; }

    [Required]
    public string ClaimType { get; set; }


    [Required]
    public string ClaimValue { get; set; }
}