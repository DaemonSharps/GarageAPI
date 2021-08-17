using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    /// <summary>
    /// Статус записи
    /// </summary>
    public class RecordState: StateBase
    {
        /// <summary>
        /// Список записей с этим статусом
        /// </summary>
        public List<Record> Records { get; set; } = new List<Record>();
    }
}
