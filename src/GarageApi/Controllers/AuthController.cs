using ExternalApiClients.Rest;
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

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterUser(string email, [FromServices] IJwtProviderApi jwtProvider)
    {
        var request = new RegisterUserRequest
        {
            Email = $"{email}@mail.ru",
            FirstName = "fn",
            LastName = "ln",
            Patronymic = "mn",
            Password = "1"
        };
        var response = await jwtProvider.RegisterUser(request);
        return response.IsSuccessStatusCode 
            ? Ok(response.Content) 
            : BadRequest(await response.Error.GetContentAsAsync<JwtError>());
    }
}
