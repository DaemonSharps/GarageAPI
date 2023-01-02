namespace GarageDataBase.Tables;

/// <summary> Класс для поддержки soft delete </summary>
public abstract class Timestamp
{
    /// <summary> Время создания сущности </summary>
    public DateTimeOffset CreationDate { get; set; }

    /// <summary> Время последнего обновления сущности </summary>
    public DateTimeOffset LastUpdate { get; set; }

    /// <summary> Время удаления суности </summary>
    public DateTimeOffset? FinishDate { get; set; }
}