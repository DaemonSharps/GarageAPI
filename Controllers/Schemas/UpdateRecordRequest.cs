using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    public class UpdateRecordRequest
    {
        public virtual long RecordId { get; set; }
        public long CustomerId { get; set; }

        public string Time { get; set; }

        public DateTime Date { get; set; }

        public int PlaceNumber { get; set; }

        public long RecordStateId { get; set; }
    }
}
