using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Features.Identity.Models;

public class UserChangePasswordViewModel<TKey>
{
    public TKey UserId { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}