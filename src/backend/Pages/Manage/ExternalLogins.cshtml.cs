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

public class ExternalLoginsModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ExternalLoginsModel> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
    public string StatusMessage { get; set; }
    public ExternalLoginsModel(UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , IEmailSender emailSender
    , ILogger<ExternalLoginsModel> logger
    , UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _urlEncoder = urlEncoder;
    }

    [BindProperty]
    public ExternalLoginsViewModel Manage { get; set; }
    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound(string.Format("Unable to load user with ID {0}.", _userManager.GetUserId(User)));
        }

        Manage = new ExternalLoginsViewModel
        {
            CurrentLogins = await _userManager.GetLoginsAsync(user)
        };

        Manage.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(auth => Manage.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
            .ToList();

        Manage.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || Manage.CurrentLogins.Count > 1;
        Manage.StatusMessage = StatusMessage;

        return Page();
    }
}