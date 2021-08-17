using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    [Serializable]
    public class GetRecordsByFilterRequest
    {
        public DateTime Date { get; set; }

        public long Page { get; set; }

        public long PerPage { get; set; }

        public long StateId { get; set; }

        public long CustomerId { get; set; }
    }
}
