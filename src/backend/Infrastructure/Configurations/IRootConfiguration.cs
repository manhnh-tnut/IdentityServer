namespace IdentityServer.Infrastructure.Configurations;
public interface IRootConfiguration
{
    AdminConfiguration AdminConfiguration { get; }

    RegisterConfiguration RegisterConfiguration { get; }
}