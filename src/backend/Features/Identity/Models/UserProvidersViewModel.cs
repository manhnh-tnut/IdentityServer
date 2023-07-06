namespace IdentityServer.Features.Identity.Models;

public class UserProvidersViewModel<TKey>
{
    public UserProvidersViewModel()
    {
        Providers = new List<UserProviderViewModel<TKey>>();
    }

    public List<UserProviderViewModel<TKey>> Providers { get; set; }
}