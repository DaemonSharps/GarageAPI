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
    public static IServiceCollection AddRestService<TRestService>(this IServiceCollection services, Uri baseAddress)
        where TRestService : class
    {
        services
            .AddRefitClient<TRestService>()
            .ConfigureHttpClient(c => c.BaseAddress = baseAddress);
        return services;
    }

    public static IServiceCollection AddJwtProviderClient(this IServiceCollection services)
        => services.AddRestService<IJwtProviderApi>(new Uri(Environment.GetEnvironmentVariable("JWT_PROVIDER_URI")));
}
