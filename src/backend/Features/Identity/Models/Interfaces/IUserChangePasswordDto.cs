namespace IdentityServer.Features.Identity.Models.Interfaces;

public interface IUserChangePasswordDto : IBaseUserChangePasswordDto
{
    string UserName { get; set; }
    string Password { get; set; }
    string ConfirmPassword { get; set; }
}