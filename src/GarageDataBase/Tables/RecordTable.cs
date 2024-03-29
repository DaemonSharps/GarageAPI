﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GarageDataBase.Tables;

/// <summary>
/// Запись
/// </summary>
public class RecordTable : Timestamp
{
    /// <summary>
    /// Id записи
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Id пользователя
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// Записавшийся пользователь
    /// </summary>
    public UserTable User { get; set; }

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
    public long StateId { get; set; }

    /// <summary>
    /// Статус записи
    /// </summary>
    [JsonIgnore]
    public RecordStateTable State { get; set; }
}
