using ExternalApiClients.Rest;
using GarageAPI.Controllers.Schemas;
using System.Globalization;
using System.Net.Http.Json;
using DTO = GarageDataBase.DTO;

namespace GarageApiIntegration.Extentions;

public static partial class HttpClientExtentions
{
    public static async Task<TokenResponse> RegisterUser(this HttpClient client, RegisterUserRequest request)
    {
        var response = await client.PostAsJsonAsync("/api/auth", request);
        response.EnsureSuccessStatusCode();
        var accessToken = await response.Content.ReadFromJsonAsync<string>();
        var refreshToken = response.Content.Headers.FirstOrDefault(h => h.Key == "Set-Cookie").Value;

        return new TokenResponse
        { 
            AccessToken = accessToken 
        };
    }
}

