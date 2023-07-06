namespace IdentityServer.Features.Identity.Models;

public class UserClaimsViewModel<TKey>
{
    public UserClaimsViewModel()
    {
        Claims = new List<UserClaimViewModel<TKey>>();
    }

    public List<UserClaimViewModel<TKey>> Claims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }
}