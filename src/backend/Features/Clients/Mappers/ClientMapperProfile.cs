using AutoMapper;
using IdentityServer.Features.Clients.Models;

namespace IdentityServer.Features.Clients.Mappers;

public class ClientMapperProfile : Profile
{
    public ClientMapperProfile()
    {
        // Client
        CreateMap<ClientDto, ClientViewModel>(MemberList.Destination)
            .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
            .ReverseMap();

        CreateMap<ClientsDto, ClientsViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ClientCloneViewModel, ClientCloneDto>(MemberList.Destination)
            .ReverseMap();

        // Client Secrets
        CreateMap<ClientSecretsDto, ClientSecretViewModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClientSecretId))
            .ReverseMap();

        CreateMap<ClientSecretDto, ClientSecretViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ClientSecretsDto, ClientSecretsViewModel>(MemberList.Destination);

        // Client Properties
        CreateMap<ClientPropertiesDto, ClientPropertyViewModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClientPropertyId))
            .ReverseMap();

        CreateMap<ClientPropertyDto, ClientPropertyViewModel>(MemberList.Destination)
            .ReverseMap();

        CreateMap<ClientPropertiesDto, ClientPropertiesViewModel>(MemberList.Destination);

        // Client Claims
        CreateMap<ClientClaimsDto, ClientClaimViewModel>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ClientClaimId))
            .ReverseMap();

        CreateMap<ClientClaimDto, ClientClaimViewModel>(MemberList.Destination)
            .ReverseMap();
        CreateMap<ClientClaimsDto, ClientClaimsViewModel>(MemberList.Destination);
    }
}