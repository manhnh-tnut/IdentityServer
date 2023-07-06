using System.Text.Encodings.Web;
using IdentityServer.Data;
using IdentityServer.Infrastructure.Common;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Models.Manage;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Manage;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<IndexModel> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    public string StatusMessage { get; set; }

    [BindProperty]
    public ManageViewModel Manage { get; set; }
    public IndexModel(UserManager<ApplicationUser> userManager
    , SignInManager<ApplicationUser> signInManager
    , IEmailSender emailSender
    , ILogger<IndexModel> logger
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

        Manage = await BuildManageIndexViewModelAsync(user);

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

        var email = user.Email;
        if (Manage.Email != email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, Manage.Email);
            if (!setEmailResult.Succeeded)
            {
                throw new ApplicationException(string.Format("Unexpected error occurred setting email for user with ID {0}.", user.Id));
            }
        }

        var phoneNumber = user.PhoneNumber;
        if (Manage.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Manage.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                throw new ApplicationException(string.Format("Unexpected error occurred setting phone number for user with ID {0}.", user.Id));
            }
        }

        await UpdateUserClaimsAsync(Manage, user);

        StatusMessage = "Your profile has been updated";

        return Page();
    }

    private async Task UpdateUserClaimsAsync(ManageViewModel model, ApplicationUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var oldProfile = OpenIdClaimHelpers.ExtractProfileInfo(claims);
        var newProfile = new OpenIdProfile
        {
            Website = model.Website,
            StreetAddress = model.StreetAddress,
            Locality = model.Locality,
            PostalCode = model.PostalCode,
            Region = model.Region,
            Country = model.Country,
            FullName = model.Name,
            Profile = model.Profile
        };

        var claimsToRemove = OpenIdClaimHelpers.ExtractClaimsToRemove(oldProfile, newProfile);
        var claimsToAdd = OpenIdClaimHelpers.ExtractClaimsToAdd(oldProfile, newProfile);
        var claimsToReplace = OpenIdClaimHelpers.ExtractClaimsToReplace(claims, newProfile);

        await _userManager.RemoveClaimsAsync(user, claimsToRemove);
        await _userManager.AddClaimsAsync(user, claimsToAdd);

        foreach (var pair in claimsToReplace)
        {
            await _userManager.ReplaceClaimAsync(user, pair.Item1, pair.Item2);
        }
    }

    private async Task<ManageViewModel> BuildManageIndexViewModelAsync(ApplicationUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var profile = OpenIdClaimHelpers.ExtractProfileInfo(claims);

        var model = new ManageViewModel
        {
            Username = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsEmailConfirmed = user.EmailConfirmed,
            StatusMessage = StatusMessage,
            Name = profile.FullName,
            Website = profile.Website,
            Profile = profile.Profile,
            Country = profile.Country,
            Region = profile.Region,
            PostalCode = profile.PostalCode,
            Locality = profile.Locality,
            StreetAddress = profile.StreetAddress
        };
        return model;
    }

}