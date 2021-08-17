using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase.Tables
{
    public class Record
    {
        public long Id { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        [MaxLength(5)]
        public string Time { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int PlaceNumber { get; set; }

        [Required]
        public long RecordStateId { get; set; }

        public RecordState RecordState { get; set; }
    }
}
