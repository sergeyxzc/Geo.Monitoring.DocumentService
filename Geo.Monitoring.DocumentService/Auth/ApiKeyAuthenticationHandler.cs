using System.Text.Encodings.Web;
using Geo.Monitoring.DocumentService.Auth.Events;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Geo.Monitoring.DocumentService.Auth;

internal class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected new ApiKeyAuthenticationEvents Events
    {
        get => (ApiKeyAuthenticationEvents)base.Events!;
        set => base.Events = value;
    }

    protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new ApiKeyAuthenticationEvents());

    private AuthenticateResult FailAuthenticateResult(string message)
    {
        Logger.LogInformation(message);
        return AuthenticateResult.Fail(message);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string authorizationHeader = Request.Headers[HeaderNames.Authorization]!;
        if (!string.IsNullOrEmpty(authorizationHeader))
            return AuthenticateResult.NoResult();

        string apiKey = Request.Headers[Options.ApiKeyHeaderName]!;
        if (string.IsNullOrEmpty(apiKey))
            return  FailAuthenticateResult("ApiKey is empty.");

        apiKey = apiKey.Trim();

        try
        {
            var result = await EventValidationAsync(apiKey);
            if (result != null)
                return result;

            return FailAuthenticateResult("Failed authentication.");
        }
        catch (Exception ex)
        {
            var authenticationFailedContext = new ApiKeyAuthenticationFailedContext(Context, Scheme, Options)
            {
                Exception = ex
            };

            await Events.AuthenticationFailed(authenticationFailedContext).ConfigureAwait(true);

            return authenticationFailedContext.Result;
        }
    }

    /// <summary>
    /// Event based validation
    /// </summary>
    private async Task<AuthenticateResult?> EventValidationAsync(string apiKey)
    {
        var validateContext = new ApiKeyValidateContext(Context, Scheme, Options, apiKey);

        await Events.ValidateApiKey(validateContext);

        if (validateContext.Result == null)
            return null;

        if (validateContext.Result.Succeeded)
        {
            return validateContext.Result.Ticket == null
                ? FailAuthenticateResult("Authentication error.")
                : validateContext.Result;
        }

        return validateContext.Result.Failure != null 
            ? validateContext.Result 
            : null;
    }
}