using GarageDataBase.DTO;
using GarageDataBase.Mapping;
using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageDataBase.Extentions;

public static partial class GarageDBContextExtentions
{
    public static async Task<Customer> GetCustomer(this GarageDBContext dBContext, string email, CancellationToken cancellationToken = default)
    {
        var customer = await dBContext
            .Customers
            .Include(c => c.CustomerState)
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        return MapperHelper.Map<Customer>(customer);
    }

    public static async Task<Customer> CreateCustomer(
        this GarageDBContext dBContext,
        string email,
        string firstName,
        string secondName,
        string lastName,
        long stateId = 1,
        CancellationToken cancellationToken = default)
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
        return MapperHelper.Map<Customer>(customerEntry.Entity);

    }

    public static async Task<List<Customer>> GetCustomersByFilter(
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

        return MapperHelper.Map<List<Customer>>(customers);
    }
}

