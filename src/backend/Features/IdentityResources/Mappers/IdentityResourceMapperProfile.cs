using AutoMapper;
using IdentityServer.Features.IdentityResources.Models;

namespace IdentityServer.Features.IdentityResources.Mappers;

public class IdentityResourceMapperProfile : Profile
{
    public IdentityResourceMapperProfile()
    {
        // Identity Resources
        CreateMap<IdentityResourcesDto, IdentityResourcesViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<IdentityResourceDto, IdentityResourceViewModel>(MemberList.Destination)
            .ReverseMap();

        // Identity Resources Properties
        CreateMap<IdentityResourcePropertiesDto, IdentityResourcePropertyViewModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdentityResourcePropertyId))
            .ReverseMap();

        CreateMap<IdentityResourcePropertyDto, IdentityResourcePropertyViewModel>(MemberList.Destination);
        CreateMap<IdentityResourcePropertiesDto, IdentityResourcePropertiesViewModel>(MemberList.Destination);
    }
}