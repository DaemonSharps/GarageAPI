using System.Collections.Generic;

namespace GarageAPI.DataBase.Tables;

/// <summary>
/// Статус записи
/// </summary>
public class RecordState : StateBase
{
    /// <summary>
    /// Список записей с этим статусом
    /// </summary>
    public List<RecordTable> Records { get; set; } = new List<RecordTable>();
}
