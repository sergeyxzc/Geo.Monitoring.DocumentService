using Microsoft.AspNetCore.Authentication;

namespace Geo.Monitoring.DocumentService.Auth;

public sealed class ApiKeyValidateContext : ResultContext<ApiKeyAuthenticationOptions>
{
    public ApiKeyValidateContext(HttpContext context, AuthenticationScheme scheme, ApiKeyAuthenticationOptions options, string apiKey) : base(context, scheme, options)
    {
        ApiKey = apiKey;
    }

    public string ApiKey { get; }
}