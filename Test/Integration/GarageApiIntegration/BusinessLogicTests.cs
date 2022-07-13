using System.Globalization;
using System.Net.Http.Json;
using GarageAPI;
using GarageAPI.Controllers.Schemas;
using GarageApiIntegration.Common;
using Tables = GarageAPI.DataBase.Tables;

namespace GarageApiIntegration;

public class BusinessLogicTests : ApiTestBase
{
    public BusinessLogicTests(GarageApiTestFixture<Startup> fixture) : base(fixture) { }

    [Fact]
    public async Task CreateCustomerAndRequest()
    {
        var request = new GetOrSetCustomerRequest
        {
            Email = "test@mail.ru",
            CustomerStateId = 1,
            FirstName = "fn",
            LastName = "ln",
            SecondName = "sn"
        };
        var result = await Client.PostAsJsonAsync("/api/customers", request);
        result.EnsureSuccessStatusCode();
        var customer = await result.Content.ReadFromJsonAsync<Tables.Customer>();
        Assert.NotNull(customer);
        Assert.Equal(request.Email, customer.Email);
        Assert.Equal(request.FirstName, customer.FirstName);
        Assert.Equal(request.LastName, customer.LastName);
        Assert.Equal(request.SecondName, customer.SecondName);
        Assert.Equal(request.CustomerStateId, customer.CustomerStateId);

        var result4 = await Client.GetFromJsonAsync<List<Tables.Customer>>($"/api/customers?Email={customer.Email}&Page=1&PerPage=10");
        var customer2 = Assert.Single(result4);

        Assert.Equal(customer2.Email, customer.Email);
        Assert.Equal(customer2.FirstName, customer.FirstName);
        Assert.Equal(customer2.LastName, customer.LastName);
        Assert.Equal(customer2.SecondName, customer.SecondName);
        Assert.Equal(customer2.CustomerStateId, customer.CustomerStateId);

        var createRecordRequest = new CreateRecordRequest
        {
            CustomerId = customer.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            RecordStateId = 1,
            Time = "22:00"
        };
        var result2 = await Client.PostAsJsonAsync("/api/records", createRecordRequest);
        result2.EnsureSuccessStatusCode();
        var record = await result2.Content.ReadFromJsonAsync<Tables.Record>();
        Assert.NotNull(record);
        Assert.Equal(createRecordRequest.Date, record.Date);
        Assert.Equal(createRecordRequest.CustomerId, record.CustomerId);
        Assert.Equal(createRecordRequest.PlaceNumber, record.PlaceNumber);
        Assert.Equal(createRecordRequest.RecordStateId, record.RecordStateId);
        Assert.Equal(createRecordRequest.Time, record.Time);

        var date = createRecordRequest.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var querry = $"?CustomerId={customer.Id}&Date={date}&Page=1&PerPage=10";
        var result3 = await Client.GetFromJsonAsync<List<Tables.Record>>($"/api/records" + querry);
        Assert.NotNull(result3);
        var record2 = Assert.Single(result3);
        Assert.Equal(record2.Date, record.Date);
        Assert.Equal(record2.CustomerId, record.CustomerId);
        Assert.Equal(record2.PlaceNumber, record.PlaceNumber);
        Assert.Equal(record2.RecordStateId, record.RecordStateId);
        Assert.Equal(record2.Time, record.Time);
    }

    [Fact]
    public async Task GetCustomerAndCreateRequest()
    {
        var request = new GetOrSetCustomerRequest
        {
            FirstName = "Арсений",
            SecondName = "Васильев",
            LastName = "Тестовый",
            CustomerStateId = 1,
            Email = "ar-seny@mail.ru"
        };
        var result = await Client.PostAsJsonAsync("/api/customers", request);
        result.EnsureSuccessStatusCode();
        var customer = await result.Content.ReadFromJsonAsync<Tables.Customer>();
        Assert.NotNull(customer);
        Assert.Equal(request.Email, customer.Email);
        Assert.Equal(request.FirstName, customer.FirstName);
        Assert.Equal(request.LastName, customer.LastName);
        Assert.Equal(request.SecondName, customer.SecondName);
        Assert.Equal(request.CustomerStateId, customer.CustomerStateId);

        var createRecordRequest = new CreateRecordRequest
        {
            CustomerId = customer.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            RecordStateId = 1,
            Time = "22:00"
        };
        var result2 = await Client.PostAsJsonAsync("/api/records", createRecordRequest);
        result2.EnsureSuccessStatusCode();
        var record = await result2.Content.ReadFromJsonAsync<Tables.Record>();
        Assert.NotNull(record);
        Assert.Equal(createRecordRequest.Date, record.Date);
        Assert.Equal(createRecordRequest.CustomerId, record.CustomerId);
        Assert.Equal(createRecordRequest.PlaceNumber, record.PlaceNumber);
        Assert.Equal(createRecordRequest.RecordStateId, record.RecordStateId);
        Assert.Equal(createRecordRequest.Time, record.Time);

        var date = createRecordRequest.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var querry = $"?CustomerId={customer.Id}&Date={date}&Page=1&PerPage=10";
        var result3 = await Client.GetFromJsonAsync<List<Tables.Record>>($"/api/records" + querry);
        Assert.NotNull(result3);
        var record2 = Assert.Single(result3);
        Assert.Equal(record2.Date, record.Date);
        Assert.Equal(record2.CustomerId, record.CustomerId);
        Assert.Equal(record2.PlaceNumber, record.PlaceNumber);
        Assert.Equal(record2.RecordStateId, record.RecordStateId);
        Assert.Equal(record2.Time, record.Time);
    }

    [Fact]
    public async Task CreateNumberRequestAndGet()
    {
        var dateFrom = DateTime.Today.AddDays(2);
        for (int i = 0; i < 3; i++)
        {
            var createRecordRequest = new CreateRecordRequest
            {
                CustomerId = 1,
                Date = dateFrom.AddDays(i),
                PlaceNumber = 1,
                RecordStateId = 1,
                Time = "22:00"
            };
            var result2 = await Client.PostAsJsonAsync("/api/records", createRecordRequest);
            result2.EnsureSuccessStatusCode();
            var record = await result2.Content.ReadFromJsonAsync<Tables.Record>();
            Assert.NotNull(record);
            Assert.Equal(createRecordRequest.Date, record.Date);
            Assert.Equal(createRecordRequest.CustomerId, record.CustomerId);
            Assert.Equal(createRecordRequest.PlaceNumber, record.PlaceNumber);
            Assert.Equal(createRecordRequest.RecordStateId, record.RecordStateId);
            Assert.Equal(createRecordRequest.Time, record.Time);
        }

        var dateFromRequest = dateFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var dateToRequest = dateFrom.AddDays(3).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var querry = $"?CustomerId=1&DateFrom={dateFromRequest}&Date={dateToRequest}&Page=1&PerPage=10";
        var result3 = await Client.GetFromJsonAsync<List<Tables.Record>>($"/api/records" + querry);
        Assert.NotNull(result3);
        Assert.Equal(3, result3.Count);
    }
}
