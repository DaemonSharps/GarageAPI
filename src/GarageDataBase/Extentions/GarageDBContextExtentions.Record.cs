﻿using AutoMapper;
using GarageDataBase.DTO;
using GarageDataBase.Mapping;
using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageDataBase.Extentions;

public static partial class GarageDBContextExtentions
{
    public static async Task<List<Record>> GetRecordsBy(
        this GarageDBContext dBContext,
        int page,
        int perPage,
        DateTime dateFrom,
        DateTime dateTo,
        long recordStateId = 0,
        long customerId = 0,
        CancellationToken cancellationToken = default)
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

        var records = await recordQuerry.ToListAsync(cancellationToken);
        return MapperHelper.Map<List<Record>>(records);
    }

    public static async Task<Record> CreateRecord(
        this GarageDBContext dBContext,
        long customerId,
        string time,
        DateTime date,
        int place,
        long stateId,
        CancellationToken cancellationToken = default)
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
        await recordEntry.Reference(r => r.RecordState).LoadAsync(cancellationToken);
        await recordEntry.Reference(r => r.Customer).Query().Include(c => c.CustomerState).LoadAsync(cancellationToken);
        await dBContext.SaveChangesAsync(cancellationToken);

        return MapperHelper.Map<Record>(recordEntry.Entity);
    }

    public static async Task<Record> UpdateRecord(
        this GarageDBContext dBContext,
        long customerId,
        string time,
        DateTime? date,
        int place,
        long stateId,
        CancellationToken cancellationToken = default)
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
        await recordEntry.Reference(r => r.RecordState).LoadAsync(cancellationToken);
        await recordEntry.Reference(r => r.Customer).Query().Include(c => c.CustomerState).LoadAsync(cancellationToken);
        await dBContext.SaveChangesAsync(cancellationToken);

        return MapperHelper.Map<Record>(recordEntry.Entity);
    }
}
