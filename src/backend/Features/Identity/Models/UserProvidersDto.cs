using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class UserProvidersDto<TUserProviderDto, TKey> : UserProviderDto<TKey>, IUserProvidersDto
        where TUserProviderDto : UserProviderDto<TKey>
{
    public UserProvidersDto()
    {
        Providers = new List<TUserProviderDto>();
    }

    public List<TUserProviderDto> Providers { get; set; }

    List<IUserProviderDto> IUserProvidersDto.Providers => Providers.Cast<IUserProviderDto>().ToList();
}