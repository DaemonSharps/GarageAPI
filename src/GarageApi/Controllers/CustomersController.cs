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

namespace GarageAPI.Controllers
{
    /// <summary>
    /// Контроллер api пользователей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// Создать или получить существующего пользователя
        /// </summary>
        /// <param name="request">Запрос</param>
        [HttpPost]
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

        /// <summary>
        /// Получить пользователей по фильтру
        /// </summary>
        /// <param name="request">Запрос с фильтром</param>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<Customer>))]
        [SwaggerResponse(400, Type = typeof(string))]
        public async Task<IActionResult> Get([FromQuery] GetCustomersByFilterRequest request, [FromServices] GarageDataBase.GarageDBContext dBContext)
        {
            try
            {
                var customers = await dBContext.GetCustomersBy(
                    request.Page,
                    request.PerPage,
                    request.Email,
                    request.FirstName,
                    request.SecondName,
                    request.LastName,
                    request.VisitCount,
                    request.CustomerStateId);

                if (customers.Any())
                    return Ok(customers);
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
    }
}
