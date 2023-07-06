namespace IdentityServer.Features.Identity.Models;

public class UserProviderViewModel<TKey>
{
    public TKey UserId { get; set; }

    public string UserName { get; set; }

    public string ProviderKey { get; set; }

    public string LoginProvider { get; set; }

    public string ProviderDisplayName { get; set; }
}