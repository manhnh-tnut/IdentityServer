using AutoMapper;
using IdentityServer.Features.ApiResources.Models;

namespace IdentityServer.Features.ApiResources.Mappers;

public class ApiResourceApiMapperProfile : Profile
{
    public ApiResourceApiMapperProfile()
    {
        // Api Resources
        CreateMap<ApiResourcesDto, ApiResourcesViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ApiResourceDto, ApiResourceViewModel>(MemberList.Destination)
            .ReverseMap();

        // Api Secrets
        CreateMap<ApiSecretsDto, ApiSecretViewModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApiSecretId))
            .ReverseMap();

        CreateMap<ApiSecretDto, ApiSecretViewModel>(MemberList.Destination);
        CreateMap<ApiSecretsDto, ApiSecretsViewModel>(MemberList.Destination);

        // Api Properties
        CreateMap<ApiResourcePropertiesDto, ApiResourcePropertyViewModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApiResourcePropertyId))
            .ReverseMap();

        CreateMap<ApiResourcePropertyDto, ApiResourcePropertyViewModel>(MemberList.Destination);
        CreateMap<ApiResourcePropertiesDto, ApiResourcePropertiesViewModel>(MemberList.Destination);
    }
}