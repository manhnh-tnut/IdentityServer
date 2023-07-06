namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IRoleClaimDto : IBaseRoleClaimDto
{
    string ClaimType { get; set; }
    string ClaimValue { get; set; }
}