namespace IdentityServer.Infrastructure.Configurations;
public class AdminConfiguration
{
    public string PageTitle { get; set; }
    public string HomePageLogoUri { get; set; }
    public string FaviconUri { get; set; }
    public string IdentityAdminBaseUrl { get; set; }
    public string AdministrationRole { get; set; }
    public string Theme { get; set; }
    public string CustomThemeCss { get; set; }
    public string IdentityServerBaseUrl { get; internal set; }
    public bool RequireHttpsMetadata { get; internal set; }
    public string OidcApiName { get; internal set; }
    public string ApiVersion { get; internal set; }
    public string ApiName { get; internal set; }
    public string OidcSwaggerUIClientId { get; set; }
}