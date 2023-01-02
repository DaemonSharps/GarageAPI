using DeepEqual.Syntax;
using GarageAPI;
using GarageAPI.Controllers.Schemas;
using GarageApiIntegration.Common;
using GarageApiIntegration.Extentions;
using UserDTO = GarageDataBase.DTO.User;
using RecordDTO = GarageDataBase.DTO.Record;

namespace GarageApiIntegration;

public class BusinessLogicTests : ApiTestBase
{
    public BusinessLogicTests(GarageApiTestFixture<Startup> fixture) : base(fixture) { }

    [Fact]
    public async Task CreateUserAndRequest()
    {
        var request = new GetOrSetUserRequest
        {
            Email = "test@mail.ru",
            StateId = 1,
            FirstName = "fn",
            LastName = "ln",
            SecondName = "sn"
        };
        var user = await Client.GetOrCreateUser(request, "Clear");

        var userFilterRequest = new GetUsersByFilterRequest
        {
            Email = user.Email,
            Page = 1,
            PerPage = 10
        };
        var filteredUsers = await Client.GetUsersByFilter(userFilterRequest);
        var filteredUser = Assert.Single(filteredUsers);
        filteredUser.ShouldDeepEqual(user);

        var createRecordRequest = new CreateRecordRequest
        {
            UserId = user.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            StateId = 1,
            Time = "22:00"
        };
        var record = await Client.CreateOrUpdateRecord(createRecordRequest, user);

        var filterRequest = new GetRecordsByFilterRequest
        {
            Date = createRecordRequest.Date,
            UserId = user.Id,
            Page = 1,
            PerPage = 10
        };
        var filteredRecords = await Client.GetRecordsByFilter(filterRequest);
        Assert.NotEmpty(filteredRecords);
        var filteredRecord = Assert.Single(filteredRecords);
        filteredRecord.ShouldDeepEqual(record);
    }

    [Fact]
    public async Task GetUserAndCreateRequest()
    {
        var request = new GetOrSetUserRequest
        {
            FirstName = "Арсений",
            SecondName = "Васильев",
            LastName = "Тестовый",
            StateId = 1,
            Email = "ar-seny@mail.ru"
        };
        var user = await Client.GetOrCreateUser(request, "Clear");

        var createRecordRequest = new CreateRecordRequest
        {
            UserId = user.Id,
            Date = DateTime.Today.AddDays(1),
            PlaceNumber = 1,
            StateId = 1,
            Time = "22:00"
        };
        var record = await Client.CreateOrUpdateRecord(createRecordRequest, user);

        var filterRequest = new GetRecordsByFilterRequest
        {
            Date = createRecordRequest.Date,
            UserId = user.Id,
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
        var user = new UserDTO
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
                UserId = 1,
                Date = dateFrom.AddDays(i),
                PlaceNumber = 1,
                StateId = 1,
                Time = "22:00"
            };
            await Client.CreateOrUpdateRecord(createRecordRequest, user);
        }

        var filterRequest = new GetRecordsByFilterRequest
        {
            DateFrom = dateFrom,
            Date = dateFrom.AddDays(3),
            UserId = user.Id,
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

        var user = new UserDTO
        {
            FirstName = "Арсений",
            SecondName = "Васильев",
            LastName = "Тестовый",
            Status = "Clear",
            Email = "ar-seny@mail.ru"
        };

        var createRecordRequest = new CreateRecordRequest
        {
            UserId = 1,
            Date = dateFrom,
            PlaceNumber = 1,
            StateId = 1,
            Time = "22:00"
        };
        var createdRecord = await Client.CreateOrUpdateRecord(createRecordRequest, user);

        createRecordRequest.Time = "00:00";
        var updatedRecord = await Client.CreateOrUpdateRecord(createRecordRequest, user);

        updatedRecord.WithDeepEqual(createdRecord)
            .IgnoreProperty<RecordDTO>(r => r.Time)
            .Assert();
        Assert.NotEqual(createdRecord.Time, updatedRecord.Time);
    }
}
