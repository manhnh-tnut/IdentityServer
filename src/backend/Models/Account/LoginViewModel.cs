using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Account;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberLogin { get; set; }
    public string ReturnUrl { get; set; }
    public bool AllowRememberLogin { get; set; }
    public bool EnableLocalLogin { get; set; }
}