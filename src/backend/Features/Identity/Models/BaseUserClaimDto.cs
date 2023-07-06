using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class BaseUserClaimDto<TUserId> : IBaseUserClaimDto
{
    public int ClaimId { get; set; }

    public TUserId UserId { get; set; }

    object IBaseUserClaimDto.UserId => UserId;
}