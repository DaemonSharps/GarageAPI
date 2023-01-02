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
public class CustomersController : ControllerBase
{
    [HttpPost]
    [SwaggerOperation("Создать или получить существующего пользователя")]
    [SwaggerResponse(200, Type = typeof(Customer))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Post([FromBody] GetOrSetCustomerRequest request, [FromServices] GarageDataBase.GarageDBContext context, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await context.GetCustomer(request.Email, cancellationToken);

            if (customer == null)
            {
                customer = await context
                    .CreateCustomer(
                    request.Email,
                    request.FirstName,
                    request.SecondName,
                    request.LastName,
                    cancellationToken: cancellationToken);
            }
            return Ok(customer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [SwaggerOperation("Получить пользователей по фильтру")]
    [SwaggerResponse(200, Type = typeof(List<Customer>))]
    [SwaggerResponse(400, Type = typeof(string))]
    public async Task<IActionResult> Get([FromQuery] GetCustomersByFilterRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext, CancellationToken cancellationToken)
    {
        try
        {
            var customers = await dBContext.GetCustomersByFilter(
                request.Page,
                request.PerPage,
                request.Email,
                request.FirstName,
                request.SecondName,
                request.LastName,
                request.VisitCount,
                request.CustomerStateId,
                cancellationToken);

            if (customers.Any())
                return Ok(customers);
            else return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }
}
