using DeepEqual.Syntax;
using GarageAPI.Controllers.Schemas;
using System.Net.Http.Json;
using DTO = GarageDataBase.DTO;
namespace GarageApiIntegration.Extentions;

public static partial class HttpClientExtentions
{
    public static async Task<DTO.User> GetOrCreateUser(this HttpClient client, GetOrSetUserRequest request, string expectedStatus)
    {
        var result = await client.PostAsJsonAsync("/users", request);
        result.EnsureSuccessStatusCode();
        var user = await result.Content.ReadFromJsonAsync<DTO.User>();

        Assert.NotNull(user);
        user.WithDeepEqual(request)
            .IgnoreSourceProperty(s => s.Id)
            .IgnoreSourceProperty(s => s.Status)
            .IgnoreDestinationProperty(d => d.StateId)
            .Assert();
        Assert.Equal(expectedStatus, user.Status);

        return user;
    }

    public static async Task<List<DTO.User>> GetUsersByFilter(this HttpClient client, GetUsersByFilterRequest request)
    {
        var querry = $"/users?Email={request.Email}&Page={request.Page}&PerPage={request.PerPage}";
        var users = await client.GetFromJsonAsync<List<DTO.User>>(querry);
        Assert.NotEmpty(users);
        return users;
    }
}

