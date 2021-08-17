using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class RecordState
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Record> Records { get; set; } = new List<Record>();
    }
}
