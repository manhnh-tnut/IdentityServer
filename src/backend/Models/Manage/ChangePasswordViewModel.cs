using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Manage;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }

    public string StatusMessage { get; set; }
}