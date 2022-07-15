using AutoMapper;
using GarageDataBase.DTO;
using GarageDataBase.Mapping;
using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageDataBase.Extentions;

public static class GarageDBContextExtentions
{
    public static async Task<Customer> GetCustomer(this GarageDBContext dBContext, string email, CancellationToken cancellationToken = default)
    {
        var customer = await dBContext
            .Customers
            .Include(c => c.CustomerState)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        return MapperHelper.CreateMapper().Map<Customer>(customer);
    }

    public static async Task<Customer> CreateCustomer(this GarageDBContext dBContext, string email, string firstName, string secondName, string lastName, long stateId = 1, CancellationToken cancellationToken = default)
    {
        var customerToCreate = new CustomerTable
        {
            Email = email,
            CustomerStateId = stateId,
            FirstName = firstName,
            LastName = lastName,
            SecondName = secondName
        };

        var customerEntry = dBContext.Customers.Add(customerToCreate);
        await customerEntry.Reference(c => c.CustomerState).LoadAsync(cancellationToken);
        await dBContext.SaveChangesAsync(cancellationToken);
        return MapperHelper.CreateMapper().Map<Customer>(customerEntry.Entity);

    }

    public static async Task<List<Customer>> GetCustomersBy(
        this GarageDBContext dBContext,
        int page,
        int perPage,
        string email = null,
        string firstName = null,
        string secondName = null,
        string lastName = null,
        long visitCount = 0,
        long stateId = 0,
        CancellationToken cancellationToken = default)
    {
        var customersQuerry = dBContext
            .Customers
            .Include(c => c.CustomerState)
            .AsQueryable();

        if (!string.IsNullOrEmpty(email))
            customersQuerry = customersQuerry.Where(c => c.Email == email);
        if (!string.IsNullOrEmpty(firstName))
            customersQuerry = customersQuerry.Where(c => c.FirstName == firstName);
        if (!string.IsNullOrEmpty(secondName))
            customersQuerry = customersQuerry.Where(c => c.SecondName == secondName);
        if (!string.IsNullOrEmpty(lastName))
            customersQuerry = customersQuerry.Where(c => c.LastName == lastName);
        if (visitCount != 0)
            customersQuerry = customersQuerry.Where(c => c.VisitCount == visitCount);
        if (stateId != 0)
            customersQuerry = customersQuerry.Where(c => c.CustomerStateId == stateId);

        var takeFromPage = (page - 1) * perPage;

        var customers = await customersQuerry
            .Skip(takeFromPage)
            .Take(perPage)
            .ToArrayAsync(cancellationToken);

        return MapperHelper.CreateMapper().Map<List<Customer>>(customers);
    }

    public static async Task<List<Record>> GetRecordsBy(
        this GarageDBContext dBContext,
        int page,
        int perPage,
        DateTime dateFrom,
        DateTime dateTo,
        long recordStateId = 0,
        long customerId = 0)
    {
        dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
        dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day + 1);

        var recordQuerry = dBContext
            .Records
            .Include(r => r.Customer)
            .ThenInclude(c => c.CustomerState)
            .Include(r => r.RecordState)
            .Where(r =>
                r.Date >= dateFrom
                && r.Date < dateTo);


        if (recordStateId != 0)
            recordQuerry = recordQuerry.Where(r => r.RecordStateId == recordStateId);

        if (customerId != 0)
            recordQuerry = recordQuerry.Where(r => r.CustomerId == customerId);

        recordQuerry = recordQuerry
            .Skip((page - 1) * perPage)
            .Take(perPage);

        var records = await recordQuerry.ToListAsync();
        return MapperHelper.CreateMapper().Map<List<Record>>(records);
    }

    public static async Task<Record> CreateRecord(this GarageDBContext dBContext, long customerId, string time, DateTime date, int place, long stateId)
    {
        var newRecord = new RecordTable
        {
            CustomerId = customerId,
            Date = date,
            PlaceNumber = place,
            RecordStateId = stateId,
            Time = time
        };
        var recordEntry = dBContext.Records.Add(newRecord);
        await recordEntry.Reference(r => r.RecordState).LoadAsync();
        await recordEntry.Reference(r => r.Customer).Query().Include(c => c.CustomerState).LoadAsync();
        await dBContext.SaveChangesAsync();

        return MapperHelper.CreateMapper().Map<Record>(recordEntry.Entity);
    }

    public static async Task<Record> UpdateRecord(this GarageDBContext dBContext, long customerId, string time, DateTime? date, int place, long stateId)
    {
        date = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
        var originalRecord = await dBContext.Records.FirstOrDefaultAsync(r => r.CustomerId == customerId && r.Date == date.Value);
        if (originalRecord == null)
            throw new NullReferenceException("Can`t find record to update");

        originalRecord.CustomerId = customerId == 0 ? originalRecord.CustomerId : customerId;
        originalRecord.Time = string.IsNullOrEmpty(time) ? originalRecord.Time : time;
        originalRecord.Date = date ?? originalRecord.Date;
        originalRecord.PlaceNumber = place == 0 ? originalRecord.PlaceNumber : place;
        originalRecord.RecordStateId = stateId == 0 ? originalRecord.RecordStateId : stateId;
        var recordEntry = dBContext.Update(originalRecord);
        await recordEntry.Reference(r => r.RecordState).LoadAsync();
        await recordEntry.Reference(r => r.Customer).Query().Include(c => c.CustomerState).LoadAsync();
        await dBContext.SaveChangesAsync();

        return MapperHelper.CreateMapper().Map<Record>(recordEntry.Entity);
    }
}

