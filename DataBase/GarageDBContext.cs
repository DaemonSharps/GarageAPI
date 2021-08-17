using GarageAPI.DataBase.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.DataBase
{
    public class GarageDBContext: DbContext
    {
        public GarageDBContext(DbContextOptions<GarageDBContext> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerState> CustomerStates { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<RecordState> RecordStates { get; set; }

        
    }
}
