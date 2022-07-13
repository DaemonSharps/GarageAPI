namespace GarageDataBase.Tables;

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
