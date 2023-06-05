namespace Geo.Monitoring.DocumentService.Auth.Events;

/// <summary>
/// This default implementation of the IBasicAuthenticationEvents may be used if the
/// application only needs to override a few of the interface methods.
/// This may be used as a base class or may be instantiated directly.
/// </summary>
public class ApiKeyAuthenticationEvents
{
    /// <summary>
    /// A delegate assigned to this property will be invoked when the authentication handler fails and encounters an exception.
    /// </summary>
    public Func<ApiKeyAuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

    /// <summary>
    /// A delegate assigned to this property will be invoked when the credentials need validation.
    /// </summary>
    /// <remarks>
    /// You must provide a delegate for this property for authentication to occur.
    /// In your delegate you should construct an authentication principal from the user details,
    /// attach it to the context.Principal property and finally call context.Success();
    /// </remarks>
    public Func<ApiKeyValidateContext, Task> OnValidateCredentials { get; set; } = context => Task.CompletedTask;

    public virtual Task AuthenticationFailed(ApiKeyAuthenticationFailedContext context) => OnAuthenticationFailed(context);

    public virtual Task ValidateApiKey(ApiKeyValidateContext context) => OnValidateCredentials(context);
}