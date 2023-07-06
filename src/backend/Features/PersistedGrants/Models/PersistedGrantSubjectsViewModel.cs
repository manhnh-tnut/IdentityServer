namespace IdentityServer.Features.PersistedGrants.Models;

public class PersistedGrantSubjectsViewModel
{
    public PersistedGrantSubjectsViewModel()
    {
        PersistedGrants = new List<PersistedGrantSubjectViewModel>();
    }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<PersistedGrantSubjectViewModel> PersistedGrants { get; set; }
}