using Geo.Monitoring.DocumentService.Auth.Events;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Geo.Monitoring.DocumentService.Auth;

public static class AuthExtensions
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(ApiKeyAuthenticationDefaults.AuthenticationScheme)
            .AddApiKey(options =>
            {
                options.Events = new ApiKeyAuthenticationEvents()
                {
                    OnValidateCredentials = async c =>
                    {
                        await c.HttpContext.RequestServices
                            .GetRequiredService<IApiKeyValidationService>()
                            .ValidationAsync(c);
                    }
                };
            });

        services.AddScoped<IApiKeyValidationService, ApiKeyValidationService>();

        services.Configure<AuthOptions>(configuration.GetSection("Auth"));
    }

    public static void AddAuthSwagger(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Description = "ApiKey must appear in header",
            Type = SecuritySchemeType.ApiKey,
            Name = "X-Geo-ApiKey",
            In = ParameterLocation.Header,
            Scheme = "ApiKeyScheme"
        });

        var key = new OpenApiSecurityScheme()
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "ApiKey"
            },
            In = ParameterLocation.Header
        };

        var requirement = new OpenApiSecurityRequirement
        {
            {
                key,
                new List<string>()
            }
        };
        options.AddSecurityRequirement(requirement);
    }
}