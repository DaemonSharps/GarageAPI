using GarageAPI.Controllers.Schemas;
using GarageDataBase.DTO;
using GarageDataBase.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

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

            var records = await dBContext.GetRecordsByFilter(
            request.Page,
            request.PerPage,
            dateFrom,
            request.Date,
            request.StateId,
            request.UserId,
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
    public async Task<IActionResult> Post([FromBody] CreateOrUpdateRecordRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        try
        {
            var record = (await dBContext
                .GetRecordsByFilter(1, 10, request.Date, request.Date, 1, request.UserId, cancellationToken))
                .FirstOrDefault();

            if (record == null)
            {
                record = await dBContext.CreateRecord(
                        request.UserId,
                        request.Time,
                        request.Date,
                        request.PlaceNumber,
                        request.StateId,
                        cancellationToken);
            }
            else
            {
                record = await dBContext.UpdateRecord(
                    request.UserId,
                    request.Time,
                    request.Date,
                    request.PlaceNumber,
                    request.StateId,
                    cancellationToken);
            }

            return Ok(record);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create"), Authorize]
    public async Task<IActionResult> CreateRecord([FromBody] CreateRecordRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);
        var user = await dBContext.GetUser(email, cancellationToken);
        if (user != null)
        {

            var record = await dBContext.CreateRecord(
                            user.Id,
                            request.Time,
                            request.Date,
                            request.PlaceNumber,
                            1,
                            cancellationToken);
            return Ok(record);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> UpdateRecord([FromBody] UpdateRecordRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        var oldRecord = await dBContext.Records.FirstOrDefaultAsync(r => r.Id == request.RecordId, cancellationToken);
        if (oldRecord != null)
        {
            var record = await dBContext.UpdateRecord(oldRecord.UserId, request.Time, oldRecord.Date, request.PlaceNumber, 1, cancellationToken);
            return Ok(record);
        }
        else
        {
            return NotFound();
        }
    }
}
