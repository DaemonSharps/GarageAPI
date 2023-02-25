using ExternalApiClients.Rest.JwtProvider;
using GarageAPI.Controllers.Schemas;
using GarageDataBase;
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

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(RegisterUserRequest request, [FromServices] IJwtProviderApi jwtProvider, [FromServices] GarageDBContext context, CancellationToken cancellationToken)
    {
        var jwtResponse = await jwtProvider.RegisterUser(request, cancellationToken);

        if (!jwtResponse.IsSuccessStatusCode)
        {
            return BadRequest(await jwtResponse.Error.GetContentAsAsync<JwtError>());
        }

        var user = await context.GetUser(request.Email, includeDeleted: true, cancellationToken);

        if (user != null)
        {
            await context.UpdateUser(request.Email, request.FirstName, request.LastName, request.Patronymic, cancellationToken: cancellationToken);
        }
        if (user == null)
        {
            await context.CreateUser(request.Email, request.FirstName, request.LastName, request.Patronymic, cancellationToken: cancellationToken);
        }

        Response.Cookies.Append("_g_rt", jwtResponse.Content.RefreshToken.ToString());
        return Ok(jwtResponse.Content.AccessToken);
    }

    [HttpPost]
    public async Task<IActionResult> Authorize(CreateSessionRequest request, [FromServices] IJwtProviderApi jwtProvider, [FromServices] GarageDBContext context, CancellationToken cancellationToken)
    {
        var jwtResponse = await jwtProvider.CreateSession(request, cancellationToken);

        if (!jwtResponse.IsSuccessStatusCode)
        {
            return BadRequest(await jwtResponse.Error.GetContentAsAsync<JwtError>());
        }

        var user = await context.GetUser(request.Email, includeDeleted: true, cancellationToken);

        if (user == null)
        {
            user = await context.CreateUser(request.Email, null, null, null, cancellationToken: cancellationToken);
        }

        Response.Cookies.Append("_g_rt", jwtResponse.Content.RefreshToken.ToString());
        return Ok(new
        {
            AccessToken = jwtResponse.Content.AccessToken,
            User = user
        });
    }

    [HttpPut]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, [FromServices] IJwtProviderApi jwtProvider, CancellationToken cancellationToken)
    {
        var jwtResponse = await jwtProvider.RefreshToken(request, cancellationToken);

        if (!jwtResponse.IsSuccessStatusCode)
        {
            return BadRequest(await jwtResponse.Error.GetContentAsAsync<JwtError>());
        }

        Response.Cookies.Append("_g_rt", jwtResponse.Content.RefreshToken.ToString());
        return Ok(jwtResponse.Content.AccessToken);
    }
}
