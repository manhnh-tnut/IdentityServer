namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IUserClaimsDto : IUserClaimDto
{
    string UserName { get; set; }
    List<IUserClaimDto> Claims { get; }
    int TotalCount { get; set; }
    int PageSize { get; set; }
}