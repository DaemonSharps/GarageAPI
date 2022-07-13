using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    /// <summary>
    /// Статус пользователя
    /// </summary>
    public class CustomerState : StateBase
    {
        /// <summary>
        /// Пользователи с этим статусом
        /// </summary>
        public List<CustomerTable> Customers { get; set; } = new List<CustomerTable>();
    }
}
