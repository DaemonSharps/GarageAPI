using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageDataBase;

/// <summary>
/// Контекст БД
/// </summary>
public class GarageDBContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public GarageDBContext(DbContextOptions options) : base(options)
    {
    }

    #region TABLES
    /// <summary>
    /// Пользователи
    /// </summary>
    public DbSet<CustomerTable> Customers { get; set; }

    /// <summary>
    /// Статусы пользователей
    /// </summary>
    public DbSet<CustomerStateTable> CustomerStates { get; set; }

    /// <summary>
    /// Записи
    /// </summary>
    public DbSet<RecordTable> Records { get; set; }

    /// <summary>
    /// Статусы записей
    /// </summary>
    public DbSet<RecordStateTable> RecordStates { get; set; }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<CustomerTable>(customer =>
        {
            customer.HasIndex(c => c.Email).IsUnique();
        });

        builder.Entity<CustomerStateTable>(cs =>
        {
            cs.HasIndex(c => c.Name).IsUnique();
        });

        builder.Entity<RecordStateTable>(rs =>
        {
            rs.HasIndex(s => s.Name).IsUnique();
        });

        #region SEED DATA
        builder.Entity<RecordStateTable>().HasData(
        new RecordStateTable[]
        {
            new RecordStateTable { Id=1, Name="Approved"},
            new RecordStateTable { Id=2, Name="Waiting"},
            new RecordStateTable { Id=3, Name="Rejected"}
        });

        builder.Entity<CustomerStateTable>().HasData(
        new CustomerStateTable[]
        {
            new CustomerStateTable { Id=1, Name="Clear"},
            new CustomerStateTable { Id=2, Name="Banned"}
        });

        builder.Entity<CustomerTable>().HasData(
        new CustomerTable[]
        {
            new CustomerTable
            {
                Id=1,
                FirstName = "Арсений",
                SecondName = "Васильев",
                LastName = "Тестовый",
                CustomerStateId = 1,
                Email = "ar-seny@mail.ru",
                VisitCount = 0
            }
        });
        #endregion
    }
}
