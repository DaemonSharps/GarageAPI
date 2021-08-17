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
    /// Контроллер api записей
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordsService _recordsService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordsService">Сервис записей</param>
        public RecordsController(IRecordsService recordsService)
        {
            _recordsService = recordsService;
        }

        /// <summary>
        /// Получить записи по фильтру
        /// </summary>
        [HttpGet]
        [SwaggerResponse(200, "Records find", typeof(List<Record>))]
        [SwaggerResponse(400, Type = typeof(string))]
        public async Task<IActionResult> Get([FromQuery] GetRecordsByFilterRequest request)
        {
            try
            {
                var records = await _recordsService.GetRecordsByFilter(
                request.Page,
                request.PerPage,
                request.Date,
                request.StateId,
                request.CustomerId);

                if (records == null || records.Length == 0)
                    return NotFound();

                foreach (var record in records)
                {
                    record.RecordState.Records = null;
                    record.Customer.Records = null;
                }

                return Ok(records);
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
        /// Обновить запись
        /// </summary>
        [HttpPut]
        [SwaggerResponse(200, "Updated record", typeof(Record))]
        [SwaggerResponse(400, Type = typeof(string))]
        public async Task<IActionResult> Put([FromBody] UpdateRecordRequest request)
        {
            try
            {
                var newRecord = new Record
                {
                    Id = request.RecordId,
                    CustomerId = request.CustomerId,
                    Date = request.Date,
                    PlaceNumber = request.PlaceNumber,
                    RecordStateId = request.RecordStateId,
                    Time = request.Time
                };

                var result = await _recordsService.UpdateRecord(newRecord);

                result.RecordState.Records = null;
                result.Customer.Records = null;

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Создать запись
        /// </summary>
        [HttpPost]
        [SwaggerResponse(200, "Created record", typeof(Record))]
        [SwaggerResponse(400, Type = typeof(string))]
        public async Task<IActionResult> Post([FromBody] CreateRecordRequest request)
        {
            try
            {
                var record = await _recordsService.CreateRecord(
                request.CustomerId,
                request.Time,
                request.Date,
                request.PlaceNumber,
                request.RecordStateId);

                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
