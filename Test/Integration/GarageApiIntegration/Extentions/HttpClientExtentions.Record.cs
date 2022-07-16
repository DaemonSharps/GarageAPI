using System;
using System.Globalization;
using System.Net.Http.Json;
using GarageAPI.Controllers.Schemas;
using DTO = GarageDataBase.DTO;
namespace GarageApiIntegration.Extentions
{
    public static partial class HttpClientExtentions
    {
        public static async Task<DTO.Record> CreateOrUpdateRecord(this HttpClient client, CreateRecordRequest request, DTO.Customer expectedCustomer)
        {
            var response = await client.PostAsJsonAsync("/api/records", request);
            response.EnsureSuccessStatusCode();
            var record = await response.Content.ReadFromJsonAsync<DTO.Record>();

            Assert.NotNull(record);
            Assert.Equal(request.Date, record.Date);
            Assert.Equal(request.CustomerId, record.Customer.Id);
            Assert.Equal(request.PlaceNumber, record.PlaceNumber);
            Assert.Equal("Approved", record.Status);
            Assert.Equal(request.Time, record.Time);

            var recordCustomer = record.Customer;
            Assert.NotNull(recordCustomer);
            Assert.Equal(recordCustomer.Email, expectedCustomer.Email);
            Assert.Equal(recordCustomer.FirstName, expectedCustomer.FirstName);
            Assert.Equal(recordCustomer.LastName, expectedCustomer.LastName);
            Assert.Equal(recordCustomer.SecondName, expectedCustomer.SecondName);
            Assert.Equal(recordCustomer.Status, expectedCustomer.Status);

            return record;
        }

        public static async Task<List<DTO.Record>> GetRecordsByFilter(this HttpClient client, GetRecordsByFilterRequest request)
        {
            var dateToString = request.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var querry = $"?CustomerId={request.CustomerId}&Date={dateToString}&Page={request.Page}&PerPage={request.PerPage}";
            if (request.DateFrom.HasValue)
            {
                var dateFromString = request.DateFrom?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                querry += $"&DateFrom={dateFromString}";
            }
            return await client.GetFromJsonAsync<List<DTO.Record>>($"/api/records" + querry);
        }
    }
}

