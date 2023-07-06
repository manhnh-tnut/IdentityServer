using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Account;

public class RegisterViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }

    public bool AllowRegistration { get; set; }
}