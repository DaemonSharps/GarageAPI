using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    /// <summary>
    /// Запрос создания записи
    /// </summary>
    public class CreateRecordRequest : UpdateRecordRequest 
    {
        /// <summary>
        /// Id записи
        /// </summary>
        public override long RecordId 
        { 
            get => 0;
        }
    }
}
