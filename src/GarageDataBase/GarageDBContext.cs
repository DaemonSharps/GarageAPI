﻿using GarageAPI.DataBase.Tables;
using Microsoft.EntityFrameworkCore;

namespace GarageAPI.DataBase;

/// <summary>
/// Контекст БД
/// </summary>
public class GarageDBContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public GarageDBContext(DbContextOptions<GarageDBContext> options)
        : base(options)
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
    public DbSet<CustomerState> CustomerStates { get; set; }

    /// <summary>
    /// Записи
    /// </summary>
    public DbSet<RecordTable> Records { get; set; }

    /// <summary>
    /// Статусы записей
    /// </summary>
    public DbSet<RecordState> RecordStates { get; set; }
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
