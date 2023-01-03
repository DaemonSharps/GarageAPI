using ExternalApiClients.Rest;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalApiClients.Extentions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddRestService<TRestService>(this IServiceCollection services, string baseAddress)
        where TRestService : class
    {
        services
            .AddRefitClient<TRestService>()
            .ConfigureHttpClient(c => 
            {
                if (!string.IsNullOrEmpty(baseAddress))
                {
                    c.BaseAddress = new Uri(baseAddress);
                }
            });
        return services;
    }

    public static IServiceCollection AddJwtProviderClient(this IServiceCollection services)
        => services.AddRestService<IJwtProviderApi>(Environment.GetEnvironmentVariable("JWT_PROVIDER_URI"));
}
