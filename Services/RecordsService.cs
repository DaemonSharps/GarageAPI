using GarageAPI.DataBase;
using GarageAPI.DataBase.Tables;
using GarageAPI.Helpers;
using GarageAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Services
{ 
    /// <summary>
    /// Сервис для обработки записей
    /// </summary>
    public class RecordsService : IRecordsService
    {
        private readonly GarageDBContext _garageDBContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="garageDBContext">Контекст БД</param>
        public RecordsService(GarageDBContext garageDBContext)
        {
            _garageDBContext = garageDBContext;
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="customerId">Id пользователя</param>
        /// <param name="time">Время записи</param>
        /// <param name="date">Дата записи</param>
        /// <param name="place">Номер места в гараже</param>
        /// <param name="stateId">Статус записи</param>
        /// <returns></returns>
        public async Task<Record> CreateRecord(long customerId, string time, DateTime date, int place, long stateId)
        {
            var newRecord = new Record
            {
                CustomerId = customerId,
                Date = date,
                PlaceNumber = place,
                RecordStateId = stateId,
                Time = time
            };
            _garageDBContext.Records.Add(newRecord);

            await _garageDBContext.SaveChangesAsync();

            return await GetRecord(newRecord.Id);
        }

        /// <summary>
        /// Получить запись
        /// </summary>
        /// <param name="id">Id записи</param>
        /// <returns>Запись</returns>
        public async Task<Record> GetRecord(long id)
        {
            return await _garageDBContext
                .Records
                .Include(r => r.RecordState)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Получить записи по фильтру
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="page">Страница</param>
        /// <param name="perPage">Записей на страницу</param>
        /// <param name="stateId">Id записи</param>
        /// <param name="customerId">Id пользователя</param>
        /// <returns>Список записей</returns>
        /// <exception cref="ArgumentException"/>
        public async Task<Record[]> GetRecordsByFilter(int page, int perPage, DateTime dateFrom, DateTime dateTo, long stateId = 0, long customerId = 0)
        {
            if (dateFrom == null || dateTo == null || page == 0 || perPage == 0)
            {
                throw new ArgumentException("Invalid request parameters ");
            }

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day + 1);

            var recordQuerry = _garageDBContext
                .Records
                .Include(r => r.Customer)
                .Include(r => r.RecordState)
                .Where(r =>
                    r.Date >= dateFrom
                    && r.Date <= dateTo);


            if (stateId != 0)
                recordQuerry = recordQuerry.Where(r => r.RecordStateId == stateId);

            if (customerId != 0)
                recordQuerry = recordQuerry.Where(r => r.CustomerId == customerId);

            recordQuerry = recordQuerry
                .Skip((page - 1) * perPage)
                .Take(perPage);

            return await recordQuerry.ToArrayAsync();
        }

        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="newRecord">Новая запись</param>
        /// <returns>Обновленная запись</returns>
        public async Task<Record> UpdateRecord(Record newRecord)
        {
            var oldRecord = await GetRecord(newRecord.Id);
            if (oldRecord == null)
                throw new NullReferenceException("Can`t find record to update");


            oldRecord.CustomerId = !DataHelper.IsDefault(newRecord.CustomerId) 
                ? newRecord.CustomerId 
                : oldRecord.CustomerId;
            oldRecord.Date = !DataHelper.IsDefault(newRecord.Date)
                ? newRecord.Date
                : oldRecord.Date;
            oldRecord.PlaceNumber = !DataHelper.IsDefault(newRecord.PlaceNumber)
                ? newRecord.PlaceNumber
                : oldRecord.PlaceNumber;
            oldRecord.RecordStateId = !DataHelper.IsDefault(newRecord.RecordStateId)
                ? newRecord.RecordStateId
                : oldRecord.RecordStateId;
            oldRecord.Time = !string.IsNullOrEmpty(newRecord.Time)
                ? newRecord.Time
                : oldRecord.Time;

            await _garageDBContext.SaveChangesAsync();

            return oldRecord;
        }
    }
}
