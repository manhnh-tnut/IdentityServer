using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class RoleClaimsDto<TRoleClaimDto, TKey> : RoleClaimDto<TKey>, IRoleClaimsDto
        where TRoleClaimDto : RoleClaimDto<TKey>
{
    public RoleClaimsDto()
    {
        Claims = new List<TRoleClaimDto>();
    }

    public string RoleName { get; set; }

    public List<TRoleClaimDto> Claims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    List<IRoleClaimDto> IRoleClaimsDto.Claims => Claims.Cast<IRoleClaimDto>().ToList();
}