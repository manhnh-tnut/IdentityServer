using System.Text.Encodings.Web;
using IdentityServer.Data;
using IdentityServer.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Manage;

public class DeletePersonalModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<DeletePersonalModel> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
    public DeletePersonalDataViewModel Personal { get; set; }
    public DeletePersonalModel(UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , IEmailSender emailSender
    , ILogger<DeletePersonalModel> logger
    , UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _urlEncoder = urlEncoder;
    }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(string.Format("Unable to load user with ID {0}.", _userManager.GetUserId(User)));
        }

        Personal = new DeletePersonalDataViewModel
        {
            RequirePassword = await _userManager.HasPasswordAsync(user)
        };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(string.Format("Unable to load user with ID {0}.", _userManager.GetUserId(User)));
        }

        Personal.RequirePassword = await _userManager.HasPasswordAsync(user);
        if (Personal.RequirePassword)
        {
            if (!await _userManager.CheckPasswordAsync(user, Personal.Password))
            {
                ModelState.AddModelError(string.Empty, "Password not correct.");
                return Page();
            }
        }

        var result = await _userManager.DeleteAsync(user);
        var userId = await _userManager.GetUserIdAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Format("Unexpected error occurred deleting user with ID {0}.", user.Id));
        }

        await _signInManager.SignOutAsync();

        _logger.LogInformation(string.Format("User with ID '{0}' deleted themselves.", userId));

        return Redirect("~/");
    }

}