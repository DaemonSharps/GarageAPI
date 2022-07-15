using System.Linq.Expressions;
using AutoMapper;
using GarageDataBase.DTO;
using GarageDataBase.Mapping;
using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageDataBase.Extentions;

public static class GarageDBContextExtentions
{
    public static async Task<Customer> GetCustomerBy(this GarageDBContext dBContext, Expression<Func<CustomerTable, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var customer = await dBContext.Customers.Include(c => c.CustomerState).FirstOrDefaultAsync(predicate, cancellationToken);
        return CreateMapper().Map<Customer>(customer);
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

        var customer = dBContext.Customers.Add(customerToCreate);
        await customer.Reference(c => c.CustomerState).LoadAsync(cancellationToken);
        await dBContext.SaveChangesAsync(cancellationToken);
        return CreateMapper().Map<Customer>(customer.Entity);

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

        return CreateMapper().Map<List<Customer>>(customers);
    }

    private static IMapper CreateMapper()
        => new MapperConfiguration(c
            => c.AddProfile<GarageDTOMappingProfile>())
        .CreateMapper();
}

