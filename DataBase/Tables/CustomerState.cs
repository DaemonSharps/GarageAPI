using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class CustomerState
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
