using Microsoft.AspNetCore.Authentication;

namespace Geo.Monitoring.DocumentService.Auth;

public static class ApiKeyAuthenticationAppBuilderExtensions
{
    public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder) 
        => builder.AddApiKey(ApiKeyAuthenticationDefaults.AuthenticationScheme);

    public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, string authenticationScheme)
        => builder.AddApiKey(authenticationScheme, configureOptions: null);

    public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, Action<ApiKeyAuthenticationOptions>? configureOptions)
        => builder.AddApiKey(ApiKeyAuthenticationDefaults.AuthenticationScheme, configureOptions);

    public static AuthenticationBuilder AddApiKey(
        this AuthenticationBuilder builder,
        string authenticationScheme,
        Action<ApiKeyAuthenticationOptions>? configureOptions)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(authenticationScheme, configureOptions);
    }
}