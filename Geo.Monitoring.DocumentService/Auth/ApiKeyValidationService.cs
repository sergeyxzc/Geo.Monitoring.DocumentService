using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Geo.Monitoring.DocumentService.Auth;

public class ApiKeyValidationService : IApiKeyValidationService
{
    private readonly AuthOptions _authOptions;
    public ApiKeyValidationService(IOptions<AuthOptions> options)
    {
        _authOptions = options.Value;
    }

    public Task ValidationAsync(ApiKeyValidateContext validateContext)
    {
        var apiKey = _authOptions.Clients?.FirstOrDefault(x => string.Equals(x.ApiKey, validateContext.ApiKey, StringComparison.InvariantCultureIgnoreCase));

        if (apiKey != null && !string.IsNullOrEmpty(apiKey.ClientId))
            Success(apiKey.ClientId, validateContext);

        return Task.CompletedTask;
    }

    private void Success(string clientId, ApiKeyValidateContext validateContext)
    {
        var apiKey = _authOptions.Clients?.FirstOrDefault(x => string.Equals(x.ApiKey, validateContext.ApiKey, StringComparison.InvariantCultureIgnoreCase));
        if (apiKey == null)
            return;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, apiKey.ClientId, ClaimValueTypes.String, validateContext.Options.ClaimsIssuer),
            new Claim(ClaimTypes.Name, apiKey.ClientId, ClaimValueTypes.String, validateContext.Options.ClaimsIssuer)
        };

        validateContext.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, validateContext.Scheme.Name));
        validateContext.Success();
    }
}