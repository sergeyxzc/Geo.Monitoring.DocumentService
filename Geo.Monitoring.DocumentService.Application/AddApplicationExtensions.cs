using Microsoft.Extensions.DependencyInjection;

namespace Geo.Monitoring.DocumentService.Application;

public static class AddApplicationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<GeoDocumentService>();
    }
}