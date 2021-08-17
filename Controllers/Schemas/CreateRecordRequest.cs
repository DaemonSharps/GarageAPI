using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Controllers.Schemas
{
    public class CreateRecordRequest : UpdateRecordRequest 
    {
        public override long RecordId 
        { 
            get => 0;
        }
    }
}
