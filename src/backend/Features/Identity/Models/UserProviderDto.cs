using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class UserProviderDto<TKey> : BaseUserProviderDto<TKey>, IUserProviderDto
{
    public string UserName { get; set; }

    public string ProviderKey { get; set; }

    public string LoginProvider { get; set; }

    public string ProviderDisplayName { get; set; }
}