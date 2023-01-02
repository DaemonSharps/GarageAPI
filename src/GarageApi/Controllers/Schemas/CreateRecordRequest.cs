using System;
using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

/// <summary>
/// Запрос для обновления записей
/// </summary>
public class CreateRecordRequest
{

    /// <summary>
    /// Новое Id пользователя
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// Новое время записи
    /// </summary>
    [Required]
    public string Time { get; set; }

    /// <summary>
    /// Новая дата записи год-день-месяц
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Новое место посадки
    /// </summary>
    [Required]
    public int PlaceNumber { get; set; }

    /// <summary>
    /// Новый Id статуса записи
    /// </summary>
    [Required]
    public long StateId { get; set; }
}
