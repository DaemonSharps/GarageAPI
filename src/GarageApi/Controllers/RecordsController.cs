using GarageAPI.Controllers.Schemas;
using GarageDataBase.Tables;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageDataBase.Extentions;

namespace GarageAPI.Controllers;

/// <summary>
/// Контроллер api записей
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class RecordsController : ControllerBase
{
    /// <summary>
    /// Получить записи по фильтру
    /// </summary>
    [HttpGet]
    [SwaggerResponse(200, "Records find", typeof(List<RecordTable>))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Get([FromQuery] GetRecordsByFilterRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext)
    {
        try
        {
            var dateFrom = request.DateFrom ?? request.Date;

            var records = await dBContext.GetRecordsBy(
            request.Page,
            request.PerPage,
            dateFrom,
            request.Date,
            request.StateId,
            request.CustomerId);

            if (records.Any())
                return Ok(records.OrderBy(r => r.Date).ToArray());
            else return NotFound();
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
    public async Task<IActionResult> Post([FromBody] CreateRecordRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext)
    {
        try
        {
            var record = (await dBContext
                .GetRecordsBy(1, 10, request.Date, request.Date, 1, request.CustomerId))
                .SingleOrDefault();

            if (record == null)
            {
                record = await dBContext.CreateRecord(
                        request.CustomerId,
                        request.Time,
                        request.Date,
                        request.PlaceNumber,
                        request.RecordStateId);
            }
            else
            {
                record = await dBContext.UpdateRecord(request.CustomerId, request.Time, request.Date, request.PlaceNumber, request.RecordStateId);
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
