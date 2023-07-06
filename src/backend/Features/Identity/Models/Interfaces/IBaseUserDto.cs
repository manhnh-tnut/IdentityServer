namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IBaseUserDto
{
    object Id { get; }
    bool IsDefaultId();
}