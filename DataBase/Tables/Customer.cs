using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class Customer
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string SecondName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(400)]
        public string Email { get; set; }

        public long VisitCount { get; set; }


        public long CustomerStateId { get; set; }

        public CustomerState CustomerState { get; set; }

        public List<Record> Records { get; set; } = new List<Record>();
    }
}
