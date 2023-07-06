namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IUserProvidersDto : IUserProviderDto
{
    List<IUserProviderDto> Providers { get; }
}