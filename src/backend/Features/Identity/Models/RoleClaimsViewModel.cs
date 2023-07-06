namespace IdentityServer.Features.Identity.Models;

public class RoleClaimsViewModel<TKey>
{
    public RoleClaimsViewModel()
    {
        Claims = new List<RoleClaimViewModel<TKey>>();
    }

    public List<RoleClaimViewModel<TKey>> Claims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}