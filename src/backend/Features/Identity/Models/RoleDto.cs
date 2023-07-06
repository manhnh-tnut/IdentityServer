using System.ComponentModel.DataAnnotations;
using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class RoleDto<TKey> : BaseRoleDto<TKey>, IRoleDto
{
    [Required]
    public string Name { get; set; }
}