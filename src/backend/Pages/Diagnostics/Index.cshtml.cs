using IdentityServer.Models.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Diagnostics;

public class IndexModel : PageModel
{
    [BindProperty]
    public DiagnosticsViewModel Diagnostics { get; set; }
    public async Task<IActionResult> OnGet()
    {
        var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection?.LocalIpAddress?.ToString() };
        if (!localAddresses.Contains(HttpContext.Connection?.RemoteIpAddress?.ToString()))
        {
            return NotFound();
        }

        Diagnostics = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
        return Page();
    }
}