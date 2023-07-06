using AutoMapper;
using IdentityServer.Features.ApiScopes.Models;

namespace IdentityServer.Features.ApiScopes.Mappers;

public class ApiScopeMapperProfile : Profile
{
    public ApiScopeMapperProfile()
    {
        // Api Scopes
        CreateMap<ApiScopesDto, ApiScopesViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ApiScopeDto, ApiScopeViewModel>(MemberList.Destination)
            .ReverseMap();

        // Api Scope Properties
        CreateMap<ApiScopePropertiesDto, ApiScopePropertiesViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ApiScopePropertyDto, ApiScopePropertyViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ApiScopePropertiesDto, ApiScopePropertyViewModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApiScopePropertyId))
            .ReverseMap();
    }
}