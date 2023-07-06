using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.EntityFramework.Storage;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Features.Shared.Resources;
using IdentityServer.Features.Shared.Resources.Interfaces;
using IdentityServer.Infrastructure.Configurations;
using IdentityServer.Infrastructure.Constants;
using IdentityServer.Infrastructure.Filters;
using IdentityServer.Infrastructure.Helpers;
using IdentityServer.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace IdentityServer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRootConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var rootConfiguration = new RootConfiguration();
        configuration.GetSection(ConfigurationConstans.AdminConfigurationKey).Bind(rootConfiguration.AdminConfiguration);
        configuration.GetSection(ConfigurationConstans.RegisterConfigurationKey).Bind(rootConfiguration.RegisterConfiguration);
        services.AddSingleton(rootConfiguration);
        return services;
    }

    public static IServiceCollection AddCustomSpa(this IServiceCollection services)
    {
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "Client/build";
        });
        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")
                , sql => sql.MigrationsAssembly(migrationsAssembly)));

        // services.AddConfigurationDbContext<AdminConfigurationDbContext>(options =>
        //     options.ConfigureDbContext = b => b.UseSqlite(
        //         configuration.GetConnectionString("DefaultConnection")
        //         , sql => sql.MigrationsAssembly(migrationsAssembly)));

        // services.AddOperationalDbContext<AdminPersistedGrantDbContext>(options =>
        //     options.ConfigureDbContext = b => b.UseSqlite(
        //         configuration.GetConnectionString("DefaultConnection")
        //         , sql => sql.MigrationsAssembly(migrationsAssembly)));

        // services.AddDataProtection()
        //     .SetApplicationName("IdentityServer")
        //     .PersistKeysToDbContext<AdminDataProtectionDbContext>();

        services.AddDatabaseDeveloperPageExceptionFilter();
        return services;
    }

    public static IServiceCollection AddCustomEmailSenders(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
        if (smtpConfiguration != null)
        {
            services.AddSingleton(smtpConfiguration);
            services.AddTransient<IEmailSender, SmtpEmailSender>();
        }
        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomIdentityServer(configuration);

        services.Configure<CookiePolicyOptions>(options =>
        {
            options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            options.Secure = CookieSecurePolicy.SameAsRequest;
            options.OnAppendCookie = cookieContext =>
                AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            options.OnDeleteCookie = cookieContext =>
                AuthenticationHelpers.CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
        });

        services.Configure<IISOptions>(iis =>
        {
            iis.AuthenticationDisplayName = "Windows";
            iis.AutomaticAuthentication = false;
        });

        var adminApiConfiguration = configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = adminApiConfiguration.IdentityServerBaseUrl;
                options.RequireHttpsMetadata = adminApiConfiguration.RequireHttpsMetadata;
                options.Audience = adminApiConfiguration.OidcApiName;
            });

        services.ConfigureApplicationCookie(config =>
        {
            config.Cookie.Name = "IdentityServer.Cookie";
            config.LoginPath = "/Account";
            config.LogoutPath = "/Account/Logout";
            config.AccessDeniedPath = "/access-denied";
        });

        return services;
    }

    private static IServiceCollection AddCustomIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection(nameof(IdentityServerOptions));
        var identityServerOptions = configurationSection.Get<IdentityServerOptions>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

        services
            .AddScoped<ApplicationSignInManager<ApplicationUser>>()
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
                configuration.GetSection(nameof(IdentityOptions)).Bind(options)
            )
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var builder = services.AddIdentityServer(options => configurationSection.Bind(options))
            .AddTestUsers(TestUsers.Users)
            .AddDeveloperSigningCredential()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 1800; // interval in seconds
            })
            .AddConfigurationStoreCache()
            .AddAspNetIdentity<ApplicationUser>();

        if (!identityServerOptions.KeyManagement.Enabled)
        {
            builder.AddCustomSigningCredential(configuration);
            builder.AddCustomValidationKey(configuration);
        }

        return builder.Services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var rootConfiguration = new RootConfiguration();
        configuration.GetSection(ConfigurationConstans.AdminConfigurationKey).Bind(rootConfiguration.AdminConfiguration);
        configuration.GetSection(ConfigurationConstans.RegisterConfigurationKey).Bind(rootConfiguration.RegisterConfiguration);

        var adminConfiguration = configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();

        services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConstants.AdministrationPolicy,
                    policy =>
                        policy.RequireAssertion(context => context.User.HasClaim(c =>
                                ((c.Type == JwtClaimTypes.Role && c.Value == adminConfiguration.AdministrationRole) ||
                                 (c.Type == $"client_{JwtClaimTypes.Role}" && c.Value == adminConfiguration.AdministrationRole))
                            ) && context.User.HasClaim(c => c.Type == JwtClaimTypes.Scope && c.Value == adminConfiguration.OidcApiName)
                        ));
            });
        return services;
    }

    public static IServiceCollection AddCustomProfileService(this IServiceCollection services)
    {
        services.AddTransient<IProfileService, IdentityClaimsProfileService>();
        return services;
    }

    public static IServiceCollection AddCustomHstsOptions(this IServiceCollection services)
    {
        services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });
        return services;
    }

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

        return services;
    }

    public static IServiceCollection AddCustomRouting(this IServiceCollection services)
    {
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        return services;
    }

    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddScoped<ControllerExceptionFilterAttribute>();
        services.AddScoped<IApiErrorResources, ApiErrorResources>();

        services.AddControllers(options =>
        {
            options.EnableEndpointRouting = false;
        })
        .Services.AddRazorPages();

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var adminConfiguration = configuration.GetSection(nameof(AdminConfiguration)).Get<AdminConfiguration>();

        services.AddSwaggerGen(options =>
           {
               options.SwaggerDoc(adminConfiguration.ApiVersion ?? "v1", new OpenApiInfo { Title = adminConfiguration.ApiName ?? "IdentityServer", Version = adminConfiguration.ApiVersion ?? "v1" });

               options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
               {
                   Type = SecuritySchemeType.OAuth2,
                   Flows = new OpenApiOAuthFlows
                   {
                       AuthorizationCode = new OpenApiOAuthFlow
                       {
                           AuthorizationUrl = new Uri($"{adminConfiguration.IdentityServerBaseUrl ?? "https://localhost:5001"}/connect/authorize"),
                           TokenUrl = new Uri($"{adminConfiguration.IdentityServerBaseUrl ?? "https://localhost:5001"}/connect/token"),
                           Scopes = new Dictionary<string, string> {
                                { adminConfiguration.OidcApiName ?? "identity_server_api", adminConfiguration.ApiName ?? "IdentityServer Api" }
                           }
                       }
                   }
               });
               options.OperationFilter<AuthorizeCheckOperationFilter>();
           });
        return services;
    }
}