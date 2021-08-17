using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    /// <summary>
    /// Запрос для получения записей по фильтру
    /// </summary>
    public class GetRecordsByFilterRequest
    {
        /// <summary>
        /// Дата записи год-день-месяц
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Страница
        /// </summary>
        public long Page { get; set; }

        /// <summary>
        /// Результатов на страницу
        /// </summary>
        public long PerPage { get; set; }

        /// <summary>
        /// Id Статуса записи
        /// </summary>
        public long StateId { get; set; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public long CustomerId { get; set; }
    }
}
