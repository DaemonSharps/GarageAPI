using GarageAPI.DataBase.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Services.Interfaces
{
    public interface IRecordsService
    {
        Task<Record> CreateRecord(long customerId, string time, DateTime date, int place, long stateId);

        Task<Record[]> GetRecordsByFilter(DateTime date, long page, long perPage, long stateId = 0, long customerId = 0);

        Task<Record> GetRecord(long recordId);

        Task<Record> UpdateRecord(Record newRecord);
    }
}
