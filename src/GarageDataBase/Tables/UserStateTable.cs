namespace GarageDataBase.Tables;

/// <summary>
/// Статус пользователя
/// </summary>
public class UserStateTable : StateBase
{
    /// <summary>
    /// Пользователи с этим статусом
    /// </summary>
    public List<UserTable> Users { get; set; } = new List<UserTable>();
}
