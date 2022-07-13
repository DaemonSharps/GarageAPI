using System.Text.Json.Serialization;
using GarageAPI.Controllers.Schemas;

namespace GarageAPI.DataBase.Tables;

/// <summary>
/// Запись
/// </summary>
public class RecordTable : Record
{
    /// <summary>
    /// Id записи
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Записавшийся пользователь
    /// </summary>
    public CustomerTable Customer { get; set; }

    /// <summary>
    /// Статус записи
    /// </summary>
    [JsonIgnore]
    public RecordState RecordState { get; set; }
}
