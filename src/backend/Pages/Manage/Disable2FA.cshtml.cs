
using System.Text.Encodings.Web;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Manage;

public class Disable2FAModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<Disable2FAModel> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
    public string StatusMessage { get; set; }
    public Disable2FAModel(UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , IEmailSender emailSender
    , ILogger<Disable2FAModel> logger
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

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException(string.Format("Unexpected error occured disabling 2FA for user with ID {0}.", user.Id));
        }
        return Page();
    }

    public async Task<IActionResult> OnPost(string provider)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(string.Format("Unable to load user with ID {0}.", _userManager.GetUserId(User)));
        }

        var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disable2faResult.Succeeded)
        {
            throw new ApplicationException(string.Format("Unexpected error occured disabling 2FA for user with ID {0}.", user.Id));
        }

        _logger.LogInformation(string.Format("User with ID {0} has disabled 2fa.", user.Id));

        return RedirectToPage("TwoFactorAuthentication");
    }
}