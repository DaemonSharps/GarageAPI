﻿using System.Globalization;
using System.Net.Http.Json;
using GarageAPI;
using GarageAPI.Controllers.Schemas;
using GarageApiIntegration.Common;
using GarageApiIntegration.Extentions;
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
        var result = await Client.PostAsJsonAsync("/api/customers", request);
        result.EnsureSuccessStatusCode();
        var customer = await result.Content.ReadFromJsonAsync<CustomerDTO>();
        Assert.NotNull(customer);
        Assert.Equal(request.Email, customer.Email);
        Assert.Equal(request.FirstName, customer.FirstName);
        Assert.Equal(request.LastName, customer.LastName);
        Assert.Equal(request.SecondName, customer.SecondName);
        Assert.Equal("Clear", customer.Status);

        var result4 = await Client.GetFromJsonAsync<List<CustomerDTO>>($"/api/customers?Email={customer.Email}&Page=1&PerPage=10");
        var CustomerDTO = Assert.Single(result4);

        Assert.Equal(CustomerDTO.Email, customer.Email);
        Assert.Equal(CustomerDTO.FirstName, customer.FirstName);
        Assert.Equal(CustomerDTO.LastName, customer.LastName);
        Assert.Equal(CustomerDTO.SecondName, customer.SecondName);
        Assert.Equal("Clear", CustomerDTO.Status);

        var createRecordRequest = new CreateRecordRequest
        {
            CustomerId = customer.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            RecordStateId = 1,
            Time = "22:00"
        };
        var record = await Client.CreateOrUpdateRecord(createRecordRequest, customer);

        var date = createRecordRequest.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var querry = $"?CustomerId={customer.Id}&Date={date}&Page=1&PerPage=10";
        var result3 = await Client.GetFromJsonAsync<List<RecordDTO>>($"/api/records" + querry);
        Assert.NotNull(result3);
        var RecordDTO = Assert.Single(result3);
        Assert.Equal(RecordDTO.Date, record.Date);
        Assert.Equal(RecordDTO.Customer.Id, record.Customer.Id);
        Assert.Equal(RecordDTO.PlaceNumber, record.PlaceNumber);
        Assert.Equal(RecordDTO.Status, record.Status);
        Assert.Equal(RecordDTO.Time, record.Time);
        var recordCustomerDTO = RecordDTO.Customer;
        Assert.Equal(recordCustomerDTO.Email, customer.Email);
        Assert.Equal(recordCustomerDTO.FirstName, customer.FirstName);
        Assert.Equal(recordCustomerDTO.LastName, customer.LastName);
        Assert.Equal(recordCustomerDTO.SecondName, customer.SecondName);
        Assert.Equal("Clear", recordCustomerDTO.Status);
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
        var customer = await result.Content.ReadFromJsonAsync<CustomerDTO>();
        Assert.NotNull(customer);
        Assert.Equal(request.Email, customer.Email);
        Assert.Equal(request.FirstName, customer.FirstName);
        Assert.Equal(request.LastName, customer.LastName);
        Assert.Equal(request.SecondName, customer.SecondName);
        Assert.Equal("Clear", customer.Status);

        var createRecordRequest = new CreateRecordRequest
        {
            CustomerId = customer.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            RecordStateId = 1,
            Time = "22:00"
        };
        var record = await Client.CreateOrUpdateRecord(createRecordRequest, customer);

        var date = createRecordRequest.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var querry = $"?CustomerId={customer.Id}&Date={date}&Page=1&PerPage=10";
        var result3 = await Client.GetFromJsonAsync<List<RecordDTO>>($"/api/records" + querry);
        Assert.NotNull(result3);
        var RecordDTO = Assert.Single(result3);
        Assert.Equal(RecordDTO.Date, record.Date);
        Assert.Equal(RecordDTO.Customer.Id, record.Customer.Id);
        Assert.Equal(RecordDTO.PlaceNumber, record.PlaceNumber);
        Assert.Equal(RecordDTO.Status, record.Status);
        Assert.Equal(RecordDTO.Time, record.Time);
        var recordCustomerDTO = RecordDTO.Customer;
        Assert.Equal(recordCustomerDTO.Email, customer.Email);
        Assert.Equal(recordCustomerDTO.FirstName, customer.FirstName);
        Assert.Equal(recordCustomerDTO.LastName, customer.LastName);
        Assert.Equal(recordCustomerDTO.SecondName, customer.SecondName);
        Assert.Equal("Clear", recordCustomerDTO.Status);
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

        var dateFromRequest = dateFrom.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var dateToRequest = dateFrom.AddDays(3).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var querry = $"?CustomerId=1&DateFrom={dateFromRequest}&Date={dateToRequest}&Page=1&PerPage=10";
        var result3 = await Client.GetFromJsonAsync<List<RecordDTO>>($"/api/records" + querry);
        Assert.NotNull(result3);
        Assert.Equal(3, result3.Count);
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

        Assert.Equal(createdRecord.Date, updatedRecord.Date);
        Assert.Equal(createdRecord.Customer.Id, updatedRecord.Customer.Id);
        Assert.Equal(createdRecord.PlaceNumber, updatedRecord.PlaceNumber);
        Assert.Equal(createdRecord.Status, updatedRecord.Status);
        Assert.NotEqual(createdRecord.Time, updatedRecord.Time);
    }
}
