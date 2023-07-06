using Microsoft.Extensions.Localization;

namespace IdentityServer.Features.Identity.Controllers.Interfaces;

public interface IGenericControllerLocalizer<out T>
{
    LocalizedString this[string name] { get; }

    LocalizedString this[string name, params object[] arguments] { get; }

    IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures);
}