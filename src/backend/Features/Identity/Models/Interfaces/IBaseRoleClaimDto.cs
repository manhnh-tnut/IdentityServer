namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IBaseRoleClaimDto
{
    int ClaimId { get; set; }
    object RoleId { get; }
}