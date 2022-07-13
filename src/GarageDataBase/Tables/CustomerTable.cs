using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GarageAPI.DataBase.Tables;

/// <summary>
/// Пользователь
/// </summary>
public class CustomerTable
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string SecondName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string LastName { get; set; }

    /// <summary>
    /// Почта
    /// </summary>
    [Required]
    [MaxLength(400)]
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
    /// <summary>
    /// Статус пользователя
    /// </summary>
    [JsonIgnore]
    public CustomerStateTable CustomerState { get; set; }

    /// <summary>
    /// Записи пользователя
    /// </summary>
    [JsonIgnore]
    public List<RecordTable> Records { get; set; } = new List<RecordTable>();
}
