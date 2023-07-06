namespace IdentityServer.Features.PersistedGrants.Models;

public class PersistedGrantsViewModel
{
    public PersistedGrantsViewModel()
    {
        PersistedGrants = new List<PersistedGrantViewModel>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<PersistedGrantViewModel> PersistedGrants { get; set; }
}