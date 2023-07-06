namespace IdentityServer.Features.Identity.Models;

public class UserRoleViewModel<TKey>
{
    public TKey UserId { get; set; }

    public TKey RoleId { get; set; }
}