using System.Text;
using System.Text.Encodings.Web;
using IdentityServer.Data;
using IdentityServer.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace IdentityServer.Pages.Manage;

public class ResetAuthenticatorModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ResetAuthenticatorModel> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
    public string StatusMessage { get; set; }
    public ResetAuthenticatorModel(UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , IEmailSender emailSender
    , ILogger<ResetAuthenticatorModel> logger
    , UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _urlEncoder = urlEncoder;
    }

    public async Task<IActionResult> OnPost(RemoveLoginViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(string.Format("Unable to load user with ID {0}.", _userManager.GetUserId(User)));
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        _logger.LogInformation(string.Format("User with id {0} has reset their authentication app key.", user.Id));

        return RedirectToPage("EnableAuthenticator");
    }
}