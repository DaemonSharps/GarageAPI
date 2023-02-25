using ExternalApiClients.Rest;
using GarageAPI;
using GarageAPI.Controllers.Schemas;
using GarageApiIntegration.Common;
using GarageApiIntegration.Extentions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageApiIntegration;

public class AuthTests : ApiTestBase
{
    public AuthTests(GarageApiTestFixture<Startup> fixture) : base(fixture) { }

    public override void UpdateServices(IServiceCollection services)
    {
        base.UpdateServices(services);

        services.Replace(ServiceDescriptor.Transient(s => CreateJwtMock()));
    }

    private static IJwtProviderApi CreateJwtMock()
    {
        var jwtProviderMock = new Mock<IJwtProviderApi>();
        jwtProviderMock.Setup(j => j.RegisterUser(It.IsAny<RegisterUserRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Refit.ApiResponse<TokenResponse>(
                new HttpResponseMessage(System.Net.HttpStatusCode.OK),
                new TokenResponse
                {
                    AccessToken = "123124124",
                    RefreshToken = new Guid("4e264e55-b16d-416b-82bb-2cbbf2aee27b")
                },
                null,
                null));
        return jwtProviderMock.Object;
    }

    [Fact(Skip = "Потом доделаем")]
    public async Task RegisterNewUser()
    {
        var registrationRequest = new RegisterUserRequest
        {
            Email = "1@mail.ru",
            FirstName = "fn",
            LastName = "ln",
            Patronymic = "sn",
            Password = "1"
        };

        var tokenModel = await Client.RegisterUser(registrationRequest);

        Assert.NotEmpty(tokenModel.AccessToken);

        var filterRequest = new GetUsersByFilterRequest
        {
            Email = registrationRequest.Email,
            Page = 1,
            PerPage = 10
        };
        var users = await Client.GetUsersByFilter(filterRequest);

        Assert.Single(users);
    }
}
