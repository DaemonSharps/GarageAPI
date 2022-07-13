using GarageAPI;
namespace GarageApiIntegration.Common;

public class ApiTestBase : IClassFixture<GarageApiTestFixture<Startup>>
{
    public HttpClient Client { get; }

    public ApiTestBase(GarageApiTestFixture<Startup> fixture)
    {
        Client = fixture.CreateClient();
    }
}
