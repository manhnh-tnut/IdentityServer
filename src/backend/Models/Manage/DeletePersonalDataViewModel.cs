using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models.Manage;

public class DeletePersonalDataViewModel
{
    public bool RequirePassword { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }
}