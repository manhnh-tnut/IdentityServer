namespace IdentityServer.Features.Identity.Models;

public class UserProviderDeleteViewModel<TKey>
{
    public TKey UserId { get; set; }

    public string ProviderKey { get; set; }

    public string LoginProvider { get; set; }
}