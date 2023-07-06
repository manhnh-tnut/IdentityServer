using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using IdentityServer.Data;
using IdentityServer.Extensions;
using IdentityServer.Infrastructure.Common;
using IdentityServer.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace IdentityServer.Pages.Account;

public class IndexModel : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IdentityOptions _identityOptions;
    private readonly IClientStore _clientStore;
    private readonly IEventService _events;

    public IndexModel(IIdentityServerInteractionService interaction,
    IAuthenticationSchemeProvider schemeProvider,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IOptions<IdentityOptions> identityOptions,
    IEventService events,
    IClientStore clientStore)
    {
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _clientStore = clientStore;
        _userManager = userManager;
        _signInManager = signInManager;
        _identityOptions = identityOptions.Value;
        _events = events;

        Login = new LoginViewModel();
        Register = new RegisterViewModel() { AllowRegistration = AccountOptions.AllowRegistration };
    }

    [BindProperty]
    public LoginViewModel Login { get; set; }

    [BindProperty]
    public RegisterViewModel Register { get; set; }

    public async Task<IActionResult> OnGet(string returnUrl)
    {
        await BuildLoginViewModelAsync(returnUrl);
        return Page();
    }

    public async Task<IActionResult> OnPostLogin()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Login.ReturnUrl);
        var user = await _userManager.FindByEmailAsync(Login.Email);
        if (user != default(ApplicationUser))
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, Login.Password, Login.RememberLogin, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName));

                if (context != null)
                {
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return RedirectToPage("/Redirect", Login.ReturnUrl);
                    }

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    return Redirect(Login.ReturnUrl);
                }
                // request for a local page
                if (Url.IsLocalUrl(Login.ReturnUrl))
                {
                    return Redirect(Login.ReturnUrl);
                }

                if (string.IsNullOrEmpty(Login.ReturnUrl))
                {
                    return Redirect("~/");
                }

                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            if (result.IsLockedOut)
            {
                return Redirect("Account/Lockout");
            }
        }

        ModelState.Clear();
        await _events.RaiseAsync(new UserLoginFailureEvent(Login.Email, "invalid credentials", clientId: context?.Client.ClientId));
        ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);

        // something went wrong, show form with error
        return Page();
    }

    public async Task<IActionResult> OnPostRegister()
    {
        var returnUrl = Url.Content("~/");

        ViewData["ReturnUrl"] = returnUrl;

        var user = new ApplicationUser
        {
            Name = Register.Name,
            UserName = Register.UserName,
            Email = Register.Email
        };

        var result = await _userManager.CreateAsync(user, Register.Password);
        if (result.Succeeded)
        {
            if (_identityOptions.SignIn.RequireConfirmedAccount)
            {
                // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                // code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, HttpContext.Request.Scheme);

                // await _emailSender.SendEmailAsync(model.Email, _localizer["ConfirmEmailTitle"], _localizer["ConfirmEmailBody", HtmlEncoder.Default.Encode(callbackUrl)]);

                return Redirect("RegisterConfirmation");
            }
            else
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }

        ModelState.Clear();
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return Page();
    }

    private async Task BuildLoginViewModelAsync(string returnUrl)
    {
        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
        {
            var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

            // this is meant to short circuit the UI and only trigger the one external IdP
            var vm = new LoginViewModel
            {
                EnableLocalLogin = local,
                ReturnUrl = returnUrl,
                Email = context?.LoginHint,
            };
            Login.EnableLocalLogin = local;
            Login.ReturnUrl = returnUrl;
        }

        var allowLocal = true;
        if (context?.Client.ClientId != null)
        {
            var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;
            }
        }

        Login.AllowRememberLogin = AccountOptions.AllowRememberLogin;
        Login.EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin;
        Login.Email = context?.LoginHint;
    }
}