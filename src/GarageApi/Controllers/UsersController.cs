using GarageAPI.Controllers.Schemas;
using GarageDataBase.DTO;
using GarageDataBase.Extentions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GarageAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    [HttpPost]
    [SwaggerOperation("Создать или получить существующего пользователя")]
    [SwaggerResponse(200, Type = typeof(User))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Post([FromBody] GetOrSetUserRequest request, [FromServices] GarageDataBase.GarageDBContext context, CancellationToken cancellationToken)
    {
        try
        {
            var user = await context.GetUser(request.Email, includeDeleted: true, cancellationToken);

            if (user == null)
            {
                user = await context
                    .CreateUser(
                    request.Email,
                    request.FirstName,
                    request.LastName,
                    request.Patronymic,
                    cancellationToken: cancellationToken);
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [SwaggerOperation("Получить пользователей по фильтру")]
    [SwaggerResponse(200, Type = typeof(List<User>))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Get([FromQuery] GetUsersByFilterRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        try
        {
            var users = await dBContext.GetUsersByFilter(
                request.Page,
                request.PerPage,
                request.Email,
                request.FirstName,
                request.LastName,
                request.Patronymic,
                request.VisitCount,
                request.StateId,
                cancellationToken);

            if (users.Any())
                return Ok(users);
            else return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
