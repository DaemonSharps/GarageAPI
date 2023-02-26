using System;
using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

public class CreateRecordRequest
{
    /// <summary>
    /// Время записи
    /// </summary>
    [Required]
    public string Time { get; set; }

    /// <summary>
    /// Дата записи год-день-месяц
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Место посадки
    /// </summary>
    [Required]
    public int PlaceNumber { get; set; }
}

