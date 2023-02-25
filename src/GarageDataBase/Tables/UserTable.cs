using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GarageDataBase.Tables;

/// <summary>
/// Пользователь
/// </summary>
public class UserTable : Timestamp
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [MaxLength(200)]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [MaxLength(200)]
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    [MaxLength(200)]
    public string Patronymic { get; set; }

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
    public long StateId { get; set; }

    /// <summary>
    /// Статус пользователя
    /// </summary>
    [JsonIgnore]
    public UserStateTable State { get; set; }

    /// <summary>
    /// Записи пользователя
    /// </summary>
    [JsonIgnore]
    public List<RecordTable> Records { get; set; } = new List<RecordTable>();
}
