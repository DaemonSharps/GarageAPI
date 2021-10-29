using GarageAPI.Controllers.Schemas;
using GarageAPI.DataBase.Tables;
using GarageAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICustomerService _customerService;

        /// <summary>
        /// 
        /// </summary>
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Создать или получить существующего пользователя
        /// </summary>
        /// <param name="request">Запрос</param>
        [HttpPost]
        [SwaggerResponse(200, Type = typeof(ResultModel<Customer>))]
        [SwaggerResponse(400, Type = typeof(string))]
        public async Task<IActionResult> Post([FromBody] GetOrSetCustomerRequest request)
        {
            try
            {
                var model = new ResultModel<Customer>
                {
                    Action = nameof(ModelActions.Get)
                };
                var customer = (await _customerService
                .GetCustomersByFilter(1, 10, request.Email))
                .SingleOrDefault();

                if (customer == null)
                {
                    customer = await _customerService
                        .CreateCustomer(
                        request.Email,
                        request.FirstName,
                        request.SecondName,
                        request.LastName,
                        1);

                    model.Action = nameof(ModelActions.Create);
                }

                customer.CustomerState.Customers = null;
                model.Result = customer;
                return Ok(model);
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
        /// Получить пользователей по фильтру
        /// </summary>
        /// <param name="request">Запрос с фильтром</param>
        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<Customer>))]
        [SwaggerResponse(400, Type = typeof(string))]
        public async Task<IActionResult> Get([FromQuery] GetCustomersByFilterRequest request)
        {
            try
            {
                var customers = await _customerService
                .GetCustomersByFilter(
                request.Page,
                request.PerPage,
                request.Email,
                request.FirstName,
                request.SecondName,
                request.LastName,
                request.VisitCount,
                request.CustomerStateId);

                if (customers == null || customers.Length == 0)
                    return NotFound();

                foreach (var customer in customers)
                {
                    customer.CustomerState.Customers = null;
                }

                return Ok(customers);
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
