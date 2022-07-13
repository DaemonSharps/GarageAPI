using GarageAPI.DataBase.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerDTO = GarageAPI.Controllers.Schemas.Customer;

namespace GarageAPI.Services.Interfaces;

/// <summary>
/// Сервис для обработки пользователей
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Получить пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <returns>Пользователь</returns>
    Task<Customer> GetCustomer(long id);

    /// <summary>
    /// Поиск пользователей по фильтру
    /// </summary>
    /// <returns>Список пользователей</returns>
    Task<Customer[]> GetCustomersByFilter(
        int page,
        int perPage,
        string email,
        string firstName = null,
        string secondName = null,
        string lastName = null,
        long? visitCount = null,
        long stateId = 0);

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="email">Почта</param>
    /// <param name="firstName">Имя</param>
    /// <param name="secondName">Фамилия</param>
    /// <param name="lastName">Отчество</param>
    /// <param name="stateId">Id статуса</param>
    /// <returns>Созданный пользователь</returns>
    Task<CustomerDTO> CreateCustomer(
        string email,
        string firstName,
        string secondName,
        string lastName,
        long stateId);

    /// <summary>
    /// Обновить пользователя
    /// </summary>
    /// <param name="customer">Обновленный пользователь</param>
    /// <returns>Обновленный пользователь</returns>
    Task<Customer> UpdateCustomer(Customer customer);
}
