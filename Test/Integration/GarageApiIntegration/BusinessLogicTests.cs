using GarageAPI;
using GarageAPI.Controllers.Schemas;
using GarageApiIntegration.Common;
using GarageApiIntegration.Extentions;
using DeepEqual.Syntax;
using CustomerDTO = GarageDataBase.DTO.Customer;
using RecordDTO = GarageDataBase.DTO.Record;

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
        var customer = await Client.GetOrCreateCustomer(request, "Clear");

        var customerFilterRequest = new GetCustomersByFilterRequest
        {
            Email = customer.Email,
            Page = 1,
            PerPage = 10
        };
        var filteredCustomers = await Client.GetCustomersByFilter(customerFilterRequest);
        var filteredCustomer = Assert.Single(filteredCustomers);
        filteredCustomer.ShouldDeepEqual(customer);

        var createRecordRequest = new CreateRecordRequest
        {
            CustomerId = customer.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            RecordStateId = 1,
            Time = "22:00"
        };
        var record = await Client.CreateOrUpdateRecord(createRecordRequest, customer);

        var filterRequest = new GetRecordsByFilterRequest
        {
            Date = createRecordRequest.Date,
            CustomerId = customer.Id,
            Page = 1,
            PerPage = 10
        };
        var filteredRecords = await Client.GetRecordsByFilter(filterRequest);
        Assert.NotEmpty(filteredRecords);
        var filteredRecord = Assert.Single(filteredRecords);
        filteredRecord.ShouldDeepEqual(record);
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
        var customer = await Client.GetOrCreateCustomer(request, "Clear");

        var createRecordRequest = new CreateRecordRequest
        {
            CustomerId = customer.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            RecordStateId = 1,
            Time = "22:00"
        };
        var record = await Client.CreateOrUpdateRecord(createRecordRequest, customer);

        var filterRequest = new GetRecordsByFilterRequest
        {
            Date = createRecordRequest.Date,
            CustomerId = customer.Id,
            Page = 1,
            PerPage = 10
        };
        var filteredRecords = await Client.GetRecordsByFilter(filterRequest);
        var filteredRecord = Assert.Single(filteredRecords);
        filteredRecord.ShouldDeepEqual(record);
    }

    [Fact]
    public async Task CreateNumberRequestAndGet()
    {
        var customer = new CustomerDTO
        {
            FirstName = "Арсений",
            SecondName = "Васильев",
            LastName = "Тестовый",
            Status = "Clear",
            Email = "ar-seny@mail.ru"
        };
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
            await Client.CreateOrUpdateRecord(createRecordRequest, customer);
        }

        var filterRequest = new GetRecordsByFilterRequest
        {
            DateFrom = dateFrom,
            Date = dateFrom.AddDays(3),
            CustomerId = customer.Id,
            Page = 1,
            PerPage = 10
        };
        var filteredRecords = await Client.GetRecordsByFilter(filterRequest);
        Assert.Equal(3, filteredRecords.Count);
    }

    [Fact]
    public async Task UpdateExistingRecord()
    {
        var dateFrom = DateTime.Today.AddDays(2);

        var customer = new CustomerDTO
        {
            FirstName = "Арсений",
            SecondName = "Васильев",
            LastName = "Тестовый",
            Status = "Clear",
            Email = "ar-seny@mail.ru"
        };

        var createRecordRequest = new CreateRecordRequest
        {
            CustomerId = 1,
            Date = dateFrom,
            PlaceNumber = 1,
            RecordStateId = 1,
            Time = "22:00"
        };
        var createdRecord = await Client.CreateOrUpdateRecord(createRecordRequest, customer);

        createRecordRequest.Time = "00:00";
        var updatedRecord = await Client.CreateOrUpdateRecord(createRecordRequest, customer);

        updatedRecord.WithDeepEqual(createdRecord)
            .IgnoreProperty<RecordDTO>(r => r.Time)
            .Assert();
        Assert.NotEqual(createdRecord.Time, updatedRecord.Time);
    }
}
