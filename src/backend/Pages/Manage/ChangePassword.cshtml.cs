using System.Text;
using System.Text.Encodings.Web;
using IdentityServer.Data;
using IdentityServer.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Manage;

public class ChangePasswordModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ChangePasswordModel> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
    public string StatusMessage { get; set; }
    public ChangePasswordViewModel Manage { get; set; }
    public ChangePasswordModel(UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , IEmailSender emailSender
    , ILogger<ChangePasswordModel> logger
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

        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (!hasPassword)
        {
            return RedirectToPage("SetPassword");
        }

        Manage = new ChangePasswordViewModel { StatusMessage = StatusMessage };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(string.Format("Unable to load user with ID {0}.", _userManager.GetUserId(User)));
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, Manage.OldPassword, Manage.NewPassword);
        if (!changePasswordResult.Succeeded)
        {
            AddErrors(changePasswordResult);
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        _logger.LogInformation(string.Format("User {0} changed their password successfully.", user.UserName));

        StatusMessage = "Your password has been changed.";

        return Page();
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

}