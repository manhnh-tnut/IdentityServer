using System.ComponentModel.DataAnnotations;
using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class UserClaimDto<TKey> : BaseUserClaimDto<TKey>, IUserClaimDto
{
    [Required]
    public string ClaimType { get; set; }

    [Required]
    public string ClaimValue { get; set; }
}