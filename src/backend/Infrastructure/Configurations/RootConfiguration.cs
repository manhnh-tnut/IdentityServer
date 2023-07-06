namespace IdentityServer.Infrastructure.Configurations;

public class RootConfiguration : IRootConfiguration
{
    public AdminConfiguration AdminConfiguration { get; } = new AdminConfiguration();
    public RegisterConfiguration RegisterConfiguration { get; } = new RegisterConfiguration();
}