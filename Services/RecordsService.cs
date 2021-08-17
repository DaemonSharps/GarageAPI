using GarageAPI.DataBase;
using GarageAPI.DataBase.Tables;
using GarageAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Services
{
    public class RecordsService : IRecordsService
    {
        private readonly GarageDBContext _garageDBContext;

        public RecordsService(GarageDBContext garageDBContext)
        {
            _garageDBContext = garageDBContext;
        }

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

            return newRecord;
        }

        public async Task<Record> GetRecord(long id)
        {
            return await _garageDBContext
                .Records
                .Include(r => r.RecordState)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Record[]> GetRecordsByFilter(DateTime date, long page, long perPage, long stateId = 0, long customerId = 0)
        {
            if (date == null || page == 0 || perPage == 0)
            {
                throw new ArgumentException(
                    $"Invalid request parameters " +
                    $"date:{date:yyyy:dd:MM}, " +
                    $"page:{page}, " +
                    $"perPage:{perPage}");
            }

            var recordQuerry = _garageDBContext
                .Records
                .Include(r => r.Customer)
                .Include(r => r.RecordState)
                .Where(r => 
                    r.Date.Year == date.Year
                    && r.Date.Month == date.Month
                    && r.Date.Day == date.Day);


            if (stateId != 0)
                recordQuerry = recordQuerry.Where(r => r.RecordStateId == stateId);

            if (customerId != 0)
                recordQuerry = recordQuerry.Where(r => r.CustomerId == customerId);

            return await recordQuerry.ToArrayAsync();
        }

        public async Task<Record> UpdateRecord(Record newRecord)
        {
            var oldRecord = await GetRecord(newRecord.Id);
            if (oldRecord == null)
                throw new NullReferenceException("Can`t find record to update");


            oldRecord.CustomerId = !IsDefault(newRecord.CustomerId) 
                ? newRecord.CustomerId 
                : oldRecord.CustomerId;
            oldRecord.Date = !IsDefault(newRecord.Date)
                ? newRecord.Date
                : oldRecord.Date;
            oldRecord.PlaceNumber = !IsDefault(newRecord.PlaceNumber)
                ? newRecord.PlaceNumber
                : oldRecord.PlaceNumber;
            oldRecord.RecordStateId = !IsDefault(newRecord.RecordStateId)
                ? newRecord.RecordStateId
                : oldRecord.RecordStateId;

            await _garageDBContext.SaveChangesAsync();

            return oldRecord;
        }

        private bool IsDefault<T>(T value) where T : struct
        {
            object obj = Activator.CreateInstance(value.GetType());
            return obj.Equals(value);
        }
    }
}
