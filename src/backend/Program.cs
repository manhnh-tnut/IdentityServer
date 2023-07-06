using IdentityServer.Extensions;
using IdentityServer.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRootConfiguration(builder.Configuration)
    .AddCustomDbContext(builder.Configuration)
    .AddCustomEmailSenders(builder.Configuration)
    .AddCustomAuthentication(builder.Configuration)
    .AddCustomProfileService()
    .AddCustomHstsOptions()
    .AddCustomCors()
    .AddCustomRouting()
    .AddCustomControllers()
    .AddCustomAuthorization(builder.Configuration)
    .AddCustomSwagger(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    var adminConfiguration = builder.Configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();

    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", adminConfiguration.ApiName);

        c.OAuthClientId(adminConfiguration.OidcSwaggerUIClientId);
        c.OAuthAppName(adminConfiguration.ApiName);
        c.OAuthUsePkce();
    });
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCookiePolicy();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseCors();
app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();
// .RequireAuthorization();

app.MapFallbackToFile("index.html");

app.Run();
