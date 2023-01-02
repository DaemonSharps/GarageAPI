using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

/// <summary>
/// Запрос для получения пользователей по фильтру
/// </summary>
public class GetUsersByFilterRequest : GetOrSetUserRequest
{
    /// <summary>
    /// Номер страницы
    /// </summary>
    [Required]
    public int Page { get; set; }

    /// <summary>
    /// Пользователей на страницу
    /// </summary>
    [Required]
    public int PerPage { get; set; }

    /// <summary>
    /// Количество посещений
    /// </summary>
    public long VisitCount { get; set; }
}
