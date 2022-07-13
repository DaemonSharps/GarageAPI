using GarageAPI.DataBase;
using GarageAPI.DataBase.Tables;
using GarageAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using GarageAPI.Controllers.Schemas;

namespace GarageAPI.Services.Interfaces
{
    /// <summary>
    /// Сервис для обработки пользователей
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly GarageDBContext _garageBDContext;

        public CustomerService(GarageDBContext garageDBContext)
        {
            _garageBDContext = garageDBContext;
        }

        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Пользователь</returns>
        public async Task<Customer> GetCustomer(long id)
        {
            return await _garageBDContext
                .Customers
                .Include(c => c.CustomerState)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <summary>
        /// Поиск пользователей по фильтру
        /// </summary>
        /// <returns>Список пользователей</returns>
        public async Task<Customer[]> GetCustomersByFilter(
            int page,
            int perPage,
            string email = null,
            string firstName = null,
            string secondName = null,
            string lastName = null,
            long? visitCount = null,
            long stateId = 0)
        {
            if (page == 0 || perPage == 0)
            {
                throw new ArgumentException("Invalid request parameters ");
            }

            var customersQuerry = _garageBDContext
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
            if (visitCount != null)
                customersQuerry = customersQuerry.Where(c => c.VisitCount == visitCount);
            if (stateId != 0)
                customersQuerry = customersQuerry.Where(c => c.CustomerStateId == stateId);

            customersQuerry = customersQuerry
                .Skip((page - 1) * perPage)
                .Take(perPage);

            return await customersQuerry.ToArrayAsync();
        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="email">Почта</param>
        /// <param name="firstName">Имя</param>
        /// <param name="secondName">Фамилия</param>
        /// <param name="lastName">Отчество</param>
        /// <param name="stateId">Id статуса</param>
        /// <returns>Созданный пользователь</returns>
        public async Task<Customer> CreateCustomer(
            string email,
            string firstName,
            string secondName,
            string lastName,
            long stateId)
        {
            var customer = new CustomerTable
            {
                CustomerStateId = stateId,
                VisitCount = 0,
                FirstName = firstName,
                SecondName = secondName,
                LastName = lastName,
                Email = email
            };

            _garageBDContext.Customers.Add(customer);
            await _garageBDContext.SaveChangesAsync();

            customer.CustomerState = await _garageBDContext
                .CustomerStates
                .SingleOrDefaultAsync(cs => cs.Id == stateId);

            return customer;
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="newCustomer">Обновленный пользователь</param>
        /// <returns>Обновленный пользователь</returns>
        public async Task<Customer> UpdateCustomer(CustomerTable newCustomer)
        {
            var oldCustomer = await GetCustomer(newCustomer.Id);

            if (oldCustomer == null)
                throw new NullReferenceException("Can`t find customer to update");

            oldCustomer.CustomerStateId = !DataHelper.IsDefault(newCustomer.CustomerStateId)
                ? newCustomer.CustomerStateId
                : oldCustomer.CustomerStateId;
            oldCustomer.Email = !string.IsNullOrEmpty(newCustomer.Email)
                ? newCustomer.Email
                : oldCustomer.Email;
            oldCustomer.FirstName = !string.IsNullOrEmpty(newCustomer.FirstName)
                ? newCustomer.FirstName
                : oldCustomer.FirstName;
            oldCustomer.SecondName = !string.IsNullOrEmpty(newCustomer.SecondName)
                ? newCustomer.SecondName
                : oldCustomer.SecondName;
            oldCustomer.LastName = !string.IsNullOrEmpty(newCustomer.LastName)
                ? newCustomer.LastName
                : oldCustomer.LastName;
            oldCustomer.VisitCount = !DataHelper.IsDefault(newCustomer.VisitCount)
                ? newCustomer.VisitCount
                : oldCustomer.VisitCount;

            await _garageBDContext.SaveChangesAsync();

            return oldCustomer;
        }
    }
}
