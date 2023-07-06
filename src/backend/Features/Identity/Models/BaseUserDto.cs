using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class BaseUserDto<TUserId> : IBaseUserDto
{
    public TUserId Id { get; set; }

    public bool IsDefaultId() => EqualityComparer<TUserId>.Default.Equals(Id, default(TUserId));

    object IBaseUserDto.Id => Id;
}