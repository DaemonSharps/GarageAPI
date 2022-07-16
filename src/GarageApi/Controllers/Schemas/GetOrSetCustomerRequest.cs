using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

/// <summary>
/// Запрос для получения или создания пользователя
/// </summary>
public class GetOrSetCustomerRequest
{
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string SecondName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Почта
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Количество посещений
    /// </summary>
    public long VisitCount { get; set; }

    /// <summary>
    /// Id статуса
    /// </summary>
    public long CustomerStateId { get; set; }
}
