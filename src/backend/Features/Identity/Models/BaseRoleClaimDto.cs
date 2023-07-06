using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class BaseRoleClaimDto<TRoleId> : IBaseRoleClaimDto
{
    public int ClaimId { get; set; }

    public TRoleId RoleId { get; set; }

    object IBaseRoleClaimDto.RoleId => RoleId;
}