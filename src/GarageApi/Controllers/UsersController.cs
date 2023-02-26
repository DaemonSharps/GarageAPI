using ExternalApiClients.Rest;
using ExternalApiClients.Rest.JwtProvider;
using GarageAPI.Controllers.Schemas;
using GarageDataBase.DTO;
using GarageDataBase.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace GarageAPI.Controllers;

[Route("[controller]")]
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
            var user = await context.GetUser(request.Email, cancellationToken);

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

    [HttpDelete, Authorize]
    public async Task<IActionResult> Delete([FromServices] IJwtProviderApi jwtProvider, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        var accessToken = Request.Headers.Authorization.First().Split(" ")[1];
        var jwtResponse = await jwtProvider.CloseAccount(accessToken);

        if (!jwtResponse.IsSuccessStatusCode)
        {
            return BadRequest(await jwtResponse.Error.GetContentAsAsync<JwtError>());
        }

        var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);
        await dBContext.CloseUser(email, cancellationToken: cancellationToken);
        return Ok();
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> UpdateUser(UpdateUserRequest request, [FromServices] IJwtProviderApi jwtProvider, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        var accessToken = Request.Headers.Authorization.First().Split(" ")[1];
        var jwtResponse = await jwtProvider.UpdateUser(request, accessToken);

        if (!jwtResponse.IsSuccessStatusCode)
        {
            return BadRequest(await jwtResponse.Error.GetContentAsAsync<JwtError>());
        }

        var email = User.FindFirstValue(JwtRegisteredClaimNames.Email);
        await dBContext.UpdateUser(email, request.FirstName, request.LastName, request.Patronymic, cancellationToken: cancellationToken);
        return Ok();
    }
}
