using IdentityServer.Infrastructure.Enums;

namespace IdentityServer.Infrastructure.Configurations;
public class LoginConfiguration
{
    public LoginResolutionPolicy ResolutionPolicy { get; set; } = LoginResolutionPolicy.Username;
}