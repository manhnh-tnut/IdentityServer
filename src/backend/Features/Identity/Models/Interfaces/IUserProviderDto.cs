namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IUserProviderDto : IBaseUserProviderDto
{
    string UserName { get; set; }
    string ProviderKey { get; set; }
    string LoginProvider { get; set; }
    string ProviderDisplayName { get; set; }
}