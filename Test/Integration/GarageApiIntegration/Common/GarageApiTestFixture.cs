using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GarageApiIntegration.Common;

public class GarageApiTestFixture<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<TStartup>();
            })
            .ConfigureAppConfiguration((cont, conf)
                => conf.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: false));
        return builder;
    }
}
