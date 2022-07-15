namespace GarageDataBase.Tables;

/// <summary>
/// Статус записи
/// </summary>
public class RecordStateTable : StateBase
{
    /// <summary>
    /// Список записей с этим статусом
    /// </summary>
    public List<RecordTable> Records { get; set; } = new List<RecordTable>();
}
