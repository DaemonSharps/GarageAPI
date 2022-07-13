using System;
using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

/// <summary>
/// Запись
/// </summary>
public class Record
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    [Required]
    public long CustomerId { get; set; }

    /// <summary>
    /// Время записи
    /// </summary>
    [Required]
    [MaxLength(5)]
    public string Time { get; set; }

    /// <summary>
    /// Дата записи
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Номер места
    /// </summary>
    [Required]
    public int PlaceNumber { get; set; }

    /// <summary>
    /// Id статуса записи
    /// </summary>
    [Required]
    public long RecordStateId { get; set; }
}

