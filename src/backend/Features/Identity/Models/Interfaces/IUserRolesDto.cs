namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IUserRolesDto : IBaseUserRolesDto
{
    string UserName { get; set; }
    List<Shared.Models.SelectItemDto> RolesList { get; set; }
    List<IRoleDto> Roles { get; }
    int PageSize { get; set; }
    int TotalCount { get; set; }
}