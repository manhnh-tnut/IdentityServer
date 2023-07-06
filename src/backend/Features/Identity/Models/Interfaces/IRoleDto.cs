namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IRoleDto : IBaseRoleDto
{
    string Name { get; set; }
}