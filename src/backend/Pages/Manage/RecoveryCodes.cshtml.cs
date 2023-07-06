using System.Text.Encodings.Web;
using IdentityServer.Data;
using IdentityServer.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Manage;

public class RecoveryCodesModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<RecoveryCodesModel> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
    public string StatusMessage { get; set; }
    public RecoveryCodesModel(UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , IEmailSender emailSender
    , ILogger<RecoveryCodesModel> logger
    , UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _urlEncoder = urlEncoder;
    }

    [BindProperty]
    public RecoveryCodesViewModel Recovery { get; set; }

    public IActionResult OnGet()
    {
        var recoveryCodes = (string[])TempData[RecoveryCodesKey];
        if (recoveryCodes == null)
        {
            return RedirectToAction("TwoFactorAuthentication");
        }

        Recovery = new RecoveryCodesViewModel { RecoveryCodes = recoveryCodes };

        return Page();
    }

    public async Task<IActionResult> OnPost(string provider)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(string.Format("Unable to load user with ID {0}.", _userManager.GetUserId(User)));
        }

        if (!user.TwoFactorEnabled)
        {
            AddError("Cannot generate recovery codes for user as they do not have 2FA enabled.");
            return Page();
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

        _logger.LogInformation(string.Format("User with ID {0} has generated new 2FA recovery codes.", user.Id));

        var model = new RecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

        return Page();
    }

    private void AddError(string description, string title = "")
    {
        ModelState.AddModelError(title, description);
    }

}