namespace IdentityServer.Features.PersistedGrants.Models;

public class PersistedGrantsDto
{
    public PersistedGrantsDto()
    {
        PersistedGrants = new List<PersistedGrantDto>();
    }

    public string SubjectId { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public List<PersistedGrantDto> PersistedGrants { get; set; }
}