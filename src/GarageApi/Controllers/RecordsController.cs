using GarageAPI.Controllers.Schemas;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageDataBase.Extentions;
using GarageDataBase.DTO;
using System.Threading;

namespace GarageAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class RecordsController : ControllerBase
{
    [HttpGet]
    [SwaggerOperation("Получить записи по фильтру")]
    [SwaggerResponse(200, "Records find", typeof(List<Record>))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Get([FromQuery] GetRecordsByFilterRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
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
            request.CustomerId,
            cancellationToken);

            if (records.Any())
                return Ok(records.OrderBy(r => r.Date).ToArray());
            else return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost]
    [SwaggerOperation("Создать или обновить запись")]
    [SwaggerResponse(200, Type = typeof(Record))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Post([FromBody] CreateRecordRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        try
        {
            var record = (await dBContext
                .GetRecordsBy(1, 10, request.Date, request.Date, 1, request.CustomerId, cancellationToken))
                .SingleOrDefault();

            if (record == null)
            {
                record = await dBContext.CreateRecord(
                        request.CustomerId,
                        request.Time,
                        request.Date,
                        request.PlaceNumber,
                        request.RecordStateId,
                        cancellationToken);
            }
            else
            {
                record = await dBContext.UpdateRecord(
                    request.CustomerId,
                    request.Time,
                    request.Date,
                    request.PlaceNumber,
                    request.RecordStateId,
                    cancellationToken);
            }

            return Ok(record);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
