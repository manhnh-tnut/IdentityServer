namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IRolesDto
{
    int PageSize { get; set; }
    int TotalCount { get; set; }
    List<IRoleDto> Roles { get; }
}