using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class Customer
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public long VisitCount { get; set; }

        public int CustomerStateId { get; set; }

        public CustomerState CustomerState { get; set; }

        public List<Record> Records { get; set; } = new List<Record>();
    }
}
