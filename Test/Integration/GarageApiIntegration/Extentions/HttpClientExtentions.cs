using System;
using System.Net.Http.Json;
using GarageAPI.Controllers.Schemas;
using DTO = GarageDataBase.DTO;
namespace GarageApiIntegration.Extentions
{
    public static class HttpClientExtentions
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
    }
}

