using System.ComponentModel.DataAnnotations;

namespace GarageDataBase.Tables;

/// <summary>
/// Класс родитель для статусов
/// </summary>
public abstract class StateBase
{
    /// <summary>
    /// Id Статуса
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя статуса
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}
