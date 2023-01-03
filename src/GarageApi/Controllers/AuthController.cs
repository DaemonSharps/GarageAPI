using ExternalApiClients.Rest;
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
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request, [FromServices] IJwtProviderApi jwtProvider, [FromServices] GarageDBContext context, CancellationToken cancellationToken)
    {
        var response = await jwtProvider.RegisterUser(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest(await response.Error.GetContentAsAsync<JwtError>());
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

        Response.Cookies.Append("_g_rt", response.Content.RefreshToken.ToString());
        return Ok(response.Content.AccessToken);
    }
}
