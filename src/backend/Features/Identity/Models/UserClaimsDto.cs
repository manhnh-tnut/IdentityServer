using IdentityServer.Features.Identity.Models.Interfaces;

namespace IdentityServer.Features.Identity.Models;

public class UserClaimsDto<TUserClaimDto, TKey> : UserClaimDto<TKey>, IUserClaimsDto
       where TUserClaimDto : UserClaimDto<TKey>
{
    public UserClaimsDto()
    {
        Claims = new List<TUserClaimDto>();
    }

    public string UserName { get; set; }

    public List<TUserClaimDto> Claims { get; set; }

    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    List<IUserClaimDto> IUserClaimsDto.Claims => Claims.Cast<IUserClaimDto>().ToList();
}