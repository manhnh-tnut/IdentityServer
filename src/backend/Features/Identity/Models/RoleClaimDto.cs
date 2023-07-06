using System.ComponentModel.DataAnnotations;
using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class RoleClaimDto<TKey> : BaseRoleClaimDto<TKey>, IRoleClaimDto
{
    [Required]
    public string ClaimType { get; set; }


    [Required]
    public string ClaimValue { get; set; }
}