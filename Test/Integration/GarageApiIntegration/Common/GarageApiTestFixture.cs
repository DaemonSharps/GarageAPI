using ExternalApiClients.Rest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace GarageApiIntegration.Common;

public class GarageApiTestFixture<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public Action<IServiceCollection> UpdateServicesAction { private get; set; }

    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<TStartup>();
                webHostBuilder.ConfigureTestServices(UpdateServicesAction);
            })
            .ConfigureAppConfiguration((cont, conf)
                => conf.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: false));
        return builder;
    }
}
