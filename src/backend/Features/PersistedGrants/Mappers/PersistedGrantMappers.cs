using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using IdentityServer.Features.PersistedGrants.Models;
using IdentityServer.Infrastructure.Common;
using IdentityServer.Infrastructure.Entities;

namespace IdentityServer.Features.PersistedGrants.Mappers;

public static class PersistedGrantMappers
{
    static PersistedGrantMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<PersistedGrantMapperProfile>())
            .CreateMapper();
    }

    internal static IMapper Mapper { get; }

    public static T ToPersistedGrantViewModel<T>(this object source)
    {
        return Mapper.Map<T>(source);
    }

    public static PersistedGrantsDto ToModel(this PagedList<PersistedGrantDataView> grant)
    {
        return grant == null ? null : Mapper.Map<PersistedGrantsDto>(grant);
    }

    public static PersistedGrantsDto ToModel(this PagedList<PersistedGrant> grant)
    {
        return grant == null ? null : Mapper.Map<PersistedGrantsDto>(grant);
    }

    public static PersistedGrantDto ToModel(this PersistedGrant grant)
    {
        return grant == null ? null : Mapper.Map<PersistedGrantDto>(grant);
    }
}