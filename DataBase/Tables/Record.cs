using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class Record
    {
        public long Id { get; set; }

        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        public string Time { get; set; }

        public DateTime Date { get; set; }

        public int PlaceNumber { get; set; }

        public int RecordStateId { get; set; }

        public RecordState RecordState { get; set; }
    }
}
