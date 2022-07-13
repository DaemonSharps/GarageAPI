using System.Collections.Generic;

namespace GarageAPI.DataBase.Tables;

/// <summary>
/// Статус пользователя
/// </summary>
public class CustomerStateTable : StateBase
{
    /// <summary>
    /// Пользователи с этим статусом
    /// </summary>
    public List<CustomerTable> Customers { get; set; } = new List<CustomerTable>();
}
