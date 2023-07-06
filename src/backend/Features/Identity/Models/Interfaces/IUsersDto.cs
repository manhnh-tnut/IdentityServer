namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IUsersDto
{
    int PageSize { get; set; }
    int TotalCount { get; set; }
    List<IUserDto> Users { get; }
}