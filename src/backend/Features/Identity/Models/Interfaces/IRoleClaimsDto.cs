namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IRoleClaimsDto : IRoleClaimDto
{
    string RoleName { get; set; }
    List<IRoleClaimDto> Claims { get; }
    int TotalCount { get; set; }
    int PageSize { get; set; }
}