namespace Geo.Monitoring.DocumentService.Auth;

public interface IApiKeyValidationService
{
    Task ValidationAsync(ApiKeyValidateContext validateContext);
}