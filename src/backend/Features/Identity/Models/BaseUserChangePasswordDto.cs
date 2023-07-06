using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class BaseUserChangePasswordDto<TUserId> : IBaseUserChangePasswordDto
{
    public TUserId UserId { get; set; }

    object IBaseUserChangePasswordDto.UserId => UserId;
}