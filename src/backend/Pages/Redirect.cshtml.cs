using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class RedirectModel : PageModel
{
    public string RedirectUrl { get; set; }
    public void OnGet(string redirectUrl)
    {
        RedirectUrl = redirectUrl;
    }
}