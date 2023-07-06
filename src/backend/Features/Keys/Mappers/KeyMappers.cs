using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using IdentityServer.Features.Keys.Models;
using IdentityServer.Infrastructure.Common;

namespace IdentityServer.Features.Keys.Mappers;

public static class KeyMappers
{
    internal static IMapper Mapper { get; }

    static KeyMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<KeyMapperProfile>())
            .CreateMapper();
    }

    public static T ToKeyViewModel<T>(this object source)
    {
        return Mapper.Map<T>(source);
    }

    public static KeyDto ToModel(this Key key)
    {
        return key == null ? null : Mapper.Map<KeyDto>(key);
    }

    public static KeysDto ToModel(this PagedList<Key> grant)
    {
        return grant == null ? null : Mapper.Map<KeysDto>(grant);
    }
}