using GarageAPI;
using Microsoft.Extensions.DependencyInjection;

namespace GarageApiIntegration.Common;

public class ApiTestBase : IClassFixture<GarageApiTestFixture<Startup>>
{
    public HttpClient Client { get; }

    public ApiTestBase(GarageApiTestFixture<Startup> fixture)
    {
        fixture.UpdateServicesAction = UpdateServices;
        Client = fixture.CreateClient();
    }

    public virtual void UpdateServices(IServiceCollection services) { }
}
