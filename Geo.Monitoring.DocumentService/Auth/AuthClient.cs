namespace Geo.Monitoring.DocumentService.Auth;

public class AuthOptions
{
    public AuthClient[]? Clients { get; set; }
}

public class AuthClient
{
    public string ClientId { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
}