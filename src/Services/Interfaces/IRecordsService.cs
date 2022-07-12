using GarageAPI.DataBase.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Services.Interfaces
{
    /// <summary>
    /// Сервис для обработки записей
    /// </summary>
    public interface IRecordsService
    {
        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="customerId">Id пользователя</param>
        /// <param name="time">Время записи</param>
        /// <param name="date">Дата записи</param>
        /// <param name="place">Номер места в гараже</param>
        /// <param name="stateId">Статус записи</param>
        /// <returns></returns>
        Task<Record> CreateRecord(long customerId, string time, DateTime date, int place, long stateId);

        /// <summary>
        /// Получить записи по фильтру
        /// </summary>
        /// <param name="dateFrom">Дата от</param>
        /// <param name="dateTo">Дата до</param>
        /// <param name="page">Страница</param>
        /// <param name="perPage">Записей на страницу</param>
        /// <param name="stateId">Id записи</param>
        /// <param name="customerId">Id пользователя</param>
        /// <returns>Список записей</returns>
        /// <exception cref="ArgumentException"/>
        Task<Record[]> GetRecordsByFilter(int page, int perPage, DateTime dateFrom, DateTime dateTo, long stateId = 0, long customerId = 0);

        /// <summary>
        /// Получить запись
        /// </summary>
        /// <param name="recordId">Id записи</param>
        /// <returns>Запись</returns>
        Task<Record> GetRecord(long recordId);

        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="newRecord">Новая запись</param>
        /// <returns>Обновленная запись</returns>
        Task<Record> UpdateRecord(Record newRecord);
    }
}
