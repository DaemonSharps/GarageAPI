using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string SecondName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        [Required]
        [MaxLength(400)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Количество посещений
        /// </summary>
        public long VisitCount { get; set; }

        /// <summary>
        /// Id статуса
        /// </summary>
        public long CustomerStateId { get; set; }

        /// <summary>
        /// Статус пользователя
        /// </summary>
        public CustomerState CustomerState { get; set; }

        /// <summary>
        /// Записи пользователя
        /// </summary>
        public List<Record> Records { get; set; } = new List<Record>();
    }
}
