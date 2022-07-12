using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    /// <summary>
    /// Класс родиитель для статусов
    /// </summary>
    public abstract class StateBase
    {
        /// <summary>
        /// Id Статуса
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя статуса
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
