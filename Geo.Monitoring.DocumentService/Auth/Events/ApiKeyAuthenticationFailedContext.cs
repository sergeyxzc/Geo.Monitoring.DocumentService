using Microsoft.AspNetCore.Authentication;

namespace Geo.Monitoring.DocumentService.Auth.Events;

public class ApiKeyAuthenticationFailedContext : ResultContext<ApiKeyAuthenticationOptions>
{
    public ApiKeyAuthenticationFailedContext(
        HttpContext context,
        AuthenticationScheme scheme,
        ApiKeyAuthenticationOptions options)
        : base(context, scheme, options)
    {
    }

    public Exception Exception { get; set; }
}