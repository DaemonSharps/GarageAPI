using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    /// <summary>
    /// Запрос для получения пользователей по фильтру
    /// </summary>
    public class GetCustomersByFilterRequest: GetOrSetCustomerRequest 
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        [Required]
        public int Page { get; set; }

        /// <summary>
        /// Пользователей на страницу
        /// </summary>
        [Required]
        public int PerPage { get; set; }
    }
}
