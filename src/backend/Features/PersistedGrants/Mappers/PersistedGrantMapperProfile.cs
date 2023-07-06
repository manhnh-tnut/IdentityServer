using AutoMapper;
using IdentityServer.Features.PersistedGrants.Models;

namespace IdentityServer.Features.PersistedGrants.Mappers;

public class PersistedGrantMapperProfile : Profile
{
    public PersistedGrantMapperProfile()
    {
        CreateMap<PersistedGrantDto, PersistedGrantViewModel>(MemberList.Destination);
        CreateMap<PersistedGrantDto, PersistedGrantSubjectViewModel>(MemberList.Destination);
        CreateMap<PersistedGrantsDto, PersistedGrantsViewModel>(MemberList.Destination);
        CreateMap<PersistedGrantsDto, PersistedGrantSubjectsViewModel>(MemberList.Destination);
    }
}