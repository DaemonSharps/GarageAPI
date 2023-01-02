using GarageAPI.Controllers.Schemas;
using System.Globalization;
using System.Net.Http.Json;
using DTO = GarageDataBase.DTO;

namespace GarageApiIntegration.Extentions;

public static partial class HttpClientExtentions
{
    public static async Task<DTO.Record> CreateOrUpdateRecord(this HttpClient client, CreateRecordRequest request, DTO.User expectedUser)
    {
        var response = await client.PostAsJsonAsync("/api/records", request);
        response.EnsureSuccessStatusCode();
        var record = await response.Content.ReadFromJsonAsync<DTO.Record>();

        Assert.NotNull(record);
        Assert.Equal(request.Date, record.Date);
        Assert.Equal(request.UserId, record.User.Id);
        Assert.Equal(request.PlaceNumber, record.PlaceNumber);
        Assert.Equal("Approved", record.Status);
        Assert.Equal(1, record.StateId);
        Assert.Equal(request.Time, record.Time);

        var recordUser = record.User;
        Assert.NotNull(recordUser);
        Assert.Equal(recordUser.Email, expectedUser.Email);
        Assert.Equal(recordUser.FirstName, expectedUser.FirstName);
        Assert.Equal(recordUser.LastName, expectedUser.LastName);
        Assert.Equal(recordUser.Patronymic, expectedUser.Patronymic);
        Assert.Equal(recordUser.Status, expectedUser.Status);

        return record;
    }

    public static async Task<List<DTO.Record>> GetRecordsByFilter(this HttpClient client, GetRecordsByFilterRequest request)
    {
        var dateToString = request.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var querry = $"?UserId={request.UserId}&Date={dateToString}&Page={request.Page}&PerPage={request.PerPage}";
        if (request.DateFrom.HasValue)
        {
            var dateFromString = request.DateFrom?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            querry += $"&DateFrom={dateFromString}";
        }
        return await client.GetFromJsonAsync<List<DTO.Record>>($"/api/records" + querry);
    }
}

