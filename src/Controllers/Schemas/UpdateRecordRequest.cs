using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    /// <summary>
    /// Запрос создания записи
    /// </summary>
    public class UpdateRecordRequest : CreateRecordRequest
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public long RecordId { get; set; }
    }
}
