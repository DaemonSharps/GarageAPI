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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Customer>(customer =>
            {
                customer.HasIndex(c => c.Email).IsUnique();
            });

            builder.Entity<CustomerState>(cs =>
            {
                cs.HasIndex(c => c.Name).IsUnique();
            });

            builder.Entity<RecordState>(rs =>
            {
                rs.HasIndex(s => s.Name).IsUnique();
            });

            #region SEED DATA
            builder.Entity<RecordState>().HasData(
            new RecordState[]
            {
                new RecordState { Id=1, Name="Approved"},
                new RecordState { Id=2, Name="Waiting"},
                new RecordState { Id=3, Name="Rejected"}
            });

            builder.Entity<CustomerState>().HasData(
            new CustomerState[]
            {
                new CustomerState { Id=1, Name="Clear"},
                new CustomerState { Id=2, Name="Banned"}
            });

            builder.Entity<Customer>().HasData(
            new Customer[]
            {
                new Customer 
                { 
                    Id=1,
                    FirstName = "Арсений",
                    SecondName = "Васильев",
                    LastName = "",
                    CustomerStateId = 1,
                    Email = "ar-seny@mail.ru",
                    VisitCount = 0
                }
            });
            #endregion
        }
    }
}
