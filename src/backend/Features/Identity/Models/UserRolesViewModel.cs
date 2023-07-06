namespace IdentityServer.Features.Identity.Models;

public class UserRolesViewModel<TRoleDto>
{
    public UserRolesViewModel()
    {
        Roles = new List<TRoleDto>();
    }

    public List<TRoleDto> Roles { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }
}