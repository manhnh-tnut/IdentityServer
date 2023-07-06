namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IBaseUserClaimDto
{
    int ClaimId { get; set; }
    object UserId { get; }
}