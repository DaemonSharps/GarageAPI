using System.Linq.Expressions;
using AutoMapper;
using GarageDataBase.DTO;
using GarageDataBase.Mapping;
using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageDataBase.Extentions
{
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

        private static IMapper CreateMapper()
            => new MapperConfiguration(c
                => c.AddProfile<GarageDTOMappingProfile>())
            .CreateMapper();
    }
}

