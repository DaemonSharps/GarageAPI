using System.Collections.Generic;
using System.Text.Json.Serialization;
using GarageAPI.Controllers.Schemas;

namespace GarageAPI.DataBase.Tables;

/// <summary>
/// Пользователь
/// </summary>
public class CustomerTable : Customer
{
    /// <summary>
    /// Статус пользователя
    /// </summary>
    [JsonIgnore]
    public CustomerState CustomerState { get; set; }

    /// <summary>
    /// Записи пользователя
    /// </summary>
    [JsonIgnore]
    public List<Record> Records { get; set; } = new List<Record>();
}
