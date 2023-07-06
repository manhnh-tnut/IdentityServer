namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IUserClaimDto : IBaseUserClaimDto
{
    string ClaimType { get; set; }
    string ClaimValue { get; set; }
}