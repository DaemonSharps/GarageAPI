using GarageDataBase.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
    public DbSet<UserTable> Users { get; set; }

    /// <summary>
    /// Статусы пользователей
    /// </summary>
    public DbSet<UserStateTable> UserStates { get; set; }

    /// <summary>
    /// Записи
    /// </summary>
    public DbSet<RecordTable> Records { get; set; }

    /// <summary>
    /// Статусы записей
    /// </summary>
    public DbSet<RecordStateTable> RecordStates { get; set; }
    #endregion

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var changedEntries = this.ChangeTracker
            .Entries()
            .Where(e => e.Entity is Timestamp)
            .Select(e => (e.State, e.Entity as Timestamp))
            .ToArray<(EntityState State, Timestamp Timestamp)>();

        foreach (var entry in changedEntries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Timestamp.CreationDate
                    = entry.Timestamp.LastUpdate
                    = DateTimeOffset.UtcNow;
            }
            if (entry.State == EntityState.Modified)
            {
                entry.Timestamp.LastUpdate = DateTimeOffset.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    public EntityEntry Remove(Timestamp entity)
    {
        entity.FinishDate = DateTimeOffset.UtcNow;
        this.Attach(entity);
        var entry = this.Entry(entity);
        //из-за тестов
        if (entry != null)
        {
            entry.State = EntityState.Modified;
        }

        return entry;
    }

    public void RemoveRange(IEnumerable<Timestamp> entities)
    {
        foreach (var entity in entities)
        {
            Remove(entity);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserTable>(user =>
        {
            user.HasIndex(c => c.Email).IsUnique();
        });

        builder.Entity<UserStateTable>(cs =>
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

        builder.Entity<UserStateTable>().HasData(
        new UserStateTable[]
        {
            new UserStateTable { Id=1, Name="Clear"},
            new UserStateTable { Id=2, Name="Banned"}
        });

        builder.Entity<UserTable>().HasData(
        new UserTable[]
        {
            new UserTable
            {
                Id=1,
                FirstName = "Арсений",
                SecondName = "Васильев",
                LastName = "Тестовый",
                StateId = 1,
                Email = "ar-seny@mail.ru",
                VisitCount = 0
            }
        });
        #endregion
    }
}
