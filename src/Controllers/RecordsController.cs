using GarageAPI.Controllers.Schemas;
using GarageAPI.DataBase.Tables;
using GarageAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers;

/// <summary>
/// Контроллер api записей
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class RecordsController : ControllerBase
{
    private readonly IRecordsService _recordsService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="recordsService">Сервис записей</param>
    public RecordsController(IRecordsService recordsService)
    {
        _recordsService = recordsService;
    }

    /// <summary>
    /// Получить записи по фильтру
    /// </summary>
    [HttpGet]
    [SwaggerResponse(200, "Records find", typeof(List<RecordTable>))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Get([FromQuery] GetRecordsByFilterRequest request)
    {
        try
        {
            var dateFrom = request.DateFrom ?? request.Date;

            var records = await _recordsService.GetRecordsByFilter(
            request.Page,
            request.PerPage,
            dateFrom,
            request.Date,
            request.StateId,
            request.CustomerId);

            if (records == null || records.Length == 0)
                return NotFound();

            return Ok(records.OrderBy(r => r.Date).ToArray());
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    /// <summary>
    /// Создать или обновить запись
    /// </summary>
    [HttpPost]
    [SwaggerResponse(200, Type = typeof(RecordTable))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Post([FromBody] CreateRecordRequest request)
    {
        try
        {
            var record = (await _recordsService
                .GetRecordsByFilter(1, 10, request.Date, request.Date, 1, request.CustomerId))
                .SingleOrDefault();

            if (record == null)
            {
                record = await _recordsService.CreateRecord(
                        request.CustomerId,
                        request.Time,
                        request.Date,
                        request.PlaceNumber,
                        request.RecordStateId);
            }
            else
            {
                record = new RecordTable
                {
                    Id = record.Id,
                    CustomerId = request.CustomerId,
                    Time = request.Time,
                    Date = request.Date,
                    PlaceNumber = request.PlaceNumber,
                    RecordStateId = request.RecordStateId
                };
                record = await _recordsService.UpdateRecord(record);
            }

            return Ok(record);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
