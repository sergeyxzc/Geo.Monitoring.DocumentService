using Geo.Monitoring.DocumentService.Auth.Events;
using Microsoft.AspNetCore.Authentication;

namespace Geo.Monitoring.DocumentService.Auth;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// The object provided by the application to process events raised by the basic authentication middleware.
    /// The application may implement the interface fully, or it may create an instance of ApiKeyAuthenticationEvents
    /// and assign delegates only to the events it wants to process.
    /// </summary>
    public new ApiKeyAuthenticationEvents Events

    {
        get => (ApiKeyAuthenticationEvents)base.Events!;

        set => base.Events = value;
    }

    public string ApiKeyHeaderName => "X-GeoDoc-ApiKey";
    public Type? ApiKeyValidationServiceType { get; set; }
}