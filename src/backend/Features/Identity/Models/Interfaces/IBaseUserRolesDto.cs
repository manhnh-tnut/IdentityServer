namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IBaseUserRolesDto
{
    object UserId { get; }
    object RoleId { get; }
}