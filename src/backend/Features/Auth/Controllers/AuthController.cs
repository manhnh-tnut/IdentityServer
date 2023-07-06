using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Data;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication;
using Duende.IdentityServer.Models;
using IdentityServer.Extensions;
using IdentityServer.Models.Account;

namespace IdentityServer.Features.Auth.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IAuthenticationSchemeProvider _schemeProvider;
    private readonly IClientStore _clientStore;
    private readonly IEventService _events;

    public AuthController(SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IIdentityServerInteractionService interaction,
    IAuthenticationSchemeProvider schemeProvider,
    IClientStore clientStore,
    IEventService events)
    {
        _userManager = userManager;
        _interaction = interaction;
        _schemeProvider = schemeProvider;
        _clientStore = clientStore;
        _events = events;
        _signInManager = signInManager;
    }


    /// <summary>
    /// Handle postback from username/password login
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

        if (context != null)
        {
            // if the user cancels, send a result back into IdentityServer as if they 
            // denied the consent (even if this client does not require consent).
            // this will send back an access denied OIDC error response to the client.
            await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                HttpContext.Response.StatusCode = 200;
                HttpContext.Response.Headers["Location"] = "";
                return RedirectToPage("/Redirect", model.ReturnUrl);
            }

            return Redirect(model.ReturnUrl);
        }

        // since we don't have a valid context, then we just go back to the home page
        return Redirect("~/");
    }
}