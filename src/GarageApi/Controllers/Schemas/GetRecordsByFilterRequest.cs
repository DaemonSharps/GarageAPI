using System;
using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

/// <summary>
/// Запрос для получения записей по фильтру
/// </summary>
public class GetRecordsByFilterRequest
{
    /// <summary>
    /// Дата записи от год-день-месяц
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// Дата записи до год-день-месяц или конкретная дата заявок если передана только она
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Страница
    /// </summary>
    [Required]
    public int Page { get; set; }

    /// <summary>
    /// Результатов на страницу
    /// </summary>
    [Required]
    public int PerPage { get; set; }

    /// <summary>
    /// Id Статуса записи
    /// </summary>
    public long StateId { get; set; }

    /// <summary>
    /// Id пользователя
    /// </summary>
    public long UserId { get; set; }
}
