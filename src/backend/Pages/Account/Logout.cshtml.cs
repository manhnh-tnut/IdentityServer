using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Services;
using IdentityServer.Data;
using IdentityServer.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account
{
    [Authorize]
    public class LogoutModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IEventService _events;

        public LogoutModel(IIdentityServerInteractionService interaction,
        SignInManager<ApplicationUser> signInManager,
        IEventService events,
        ILogger<LogoutModel> logger)
        {
            _interaction = interaction;
            _signInManager = signInManager;
            _logger = logger;
            _events = events;

            AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut;
        }

        public async Task<IActionResult> OnGet(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false || !User.Identity.IsAuthenticated)
            {
                return await OnPost(logoutId);
            }

            return Page();
        }

        public bool AutomaticRedirectAfterSignOut { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public string SignOutIframeUrl { get; set; }
        public string ClientName { get; private set; }

        public async Task<IActionResult> OnPost(string logoutId)
        {
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context != null)
            {
                PostLogoutRedirectUri = context.PostLogoutRedirectUri;
                SignOutIframeUrl = context.SignOutIFrameUrl;
                ClientName = string.IsNullOrEmpty(context?.ClientName) ? context?.ClientId : context?.ClientName;
            }

            if (User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await _signInManager.SignOutAsync();
                // raise the logout event
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            return Page();
        }
    }
}
