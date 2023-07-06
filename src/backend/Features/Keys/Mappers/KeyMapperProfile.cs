using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using IdentityServer.Features.Keys.Models;
using IdentityServer.Infrastructure.Common;

namespace IdentityServer.Features.Keys.Mappers;

public class KeyMapperProfile : Profile
{
    public KeyMapperProfile()
    {
        CreateMap<PagedList<Key>, KeysDto>(MemberList.Destination)
            .ForMember(x => x.Keys,
                opt => opt.MapFrom(src => src.Data));

        CreateMap<Key, KeyDto>(MemberList.Destination)
            .ReverseMap();

        CreateMap<KeyDto, KeyViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<KeysDto, KeysViewModel>(MemberList.Destination)
            .ReverseMap();
    }
}