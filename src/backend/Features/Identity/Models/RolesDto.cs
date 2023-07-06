using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class RolesDto<TRoleDto, TKey> : IRolesDto where TRoleDto : RoleDto<TKey>
{
    public RolesDto()
    {
        Roles = new List<TRoleDto>();
    }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public List<TRoleDto> Roles { get; set; }

    List<IRoleDto> IRolesDto.Roles => Roles.Cast<IRoleDto>().ToList();
}