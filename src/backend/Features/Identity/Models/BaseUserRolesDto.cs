using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class BaseUserRolesDto<TKey> : IBaseUserRolesDto
{
    public TKey UserId { get; set; }

    public TKey RoleId { get; set; }

    object IBaseUserRolesDto.UserId => UserId;

    object IBaseUserRolesDto.RoleId => RoleId;
}