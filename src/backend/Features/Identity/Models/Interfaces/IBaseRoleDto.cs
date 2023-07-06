namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IBaseRoleDto
{
    object Id { get; }
    bool IsDefaultId();
}