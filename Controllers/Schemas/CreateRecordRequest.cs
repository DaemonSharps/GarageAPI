using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    /// <summary>
    /// Запрос для обновления записей
    /// </summary>
    public class CreateRecordRequest
    {

        /// <summary>
        /// Новое Id пользователя
        /// </summary>
        public long CustomerId { get; set; }

        /// <summary>
        /// Новое время записи
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Новая дата записи год-день-месяц
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Новое место посадки
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Новый Id статуса записи
        /// </summary>
        public long RecordStateId { get; set; }
    }
}
