using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class BaseRoleDto<TRoleId> : IBaseRoleDto
{
    public TRoleId Id { get; set; }

    public bool IsDefaultId() => EqualityComparer<TRoleId>.Default.Equals(Id, default(TRoleId));

    object IBaseRoleDto.Id => Id;
}