using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Orion.App.Integration.SiriusProvider.Infrastructure;

public static class DataProviderExtensions
{
    public static IServiceCollection ConfigureDataProvider(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseUri = new Uri(configuration["DataProviderSettings:BaseUri"]!);
        var timeout = TimeSpan.Parse(configuration["DataProviderSettings:Timeout"]!);
        // todo: use DataProviderSettings
        
        var refitSettings = new RefitSettings();
        services.AddRefitClient<IDataProviderApi>(refitSettings)
            .ConfigureHttpClient(v =>
            {
                v.BaseAddress = baseUri;
                v.Timeout = timeout;
            });

        return services;
    }
}