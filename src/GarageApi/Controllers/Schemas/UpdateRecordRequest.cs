using System;
using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

public class UpdateRecordRequest
{
    public long RecordId { get; set; }

    /// <summary>
    /// Время записи
    /// </summary>
    [Required]
    public string Time { get; set; }

    /// <summary>
    /// Место посадки
    /// </summary>
    [Required]
    public int PlaceNumber { get; set; }
}

