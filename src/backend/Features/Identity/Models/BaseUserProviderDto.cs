using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class BaseUserProviderDto<TUserId> : IBaseUserProviderDto
{
    public TUserId UserId { get; set; }

    object IBaseUserProviderDto.UserId => UserId;
}