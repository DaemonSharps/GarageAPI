using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class CustomerState: StateBase
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();
    }
}
