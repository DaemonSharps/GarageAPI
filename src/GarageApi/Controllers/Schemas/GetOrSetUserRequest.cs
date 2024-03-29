﻿using System.ComponentModel.DataAnnotations;

namespace GarageAPI.Controllers.Schemas;

/// <summary>
/// Запрос для получения или создания пользователя
/// </summary>
public class GetOrSetUserRequest
{
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string Patronymic { get; set; }

    /// <summary>
    /// Почта
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Id статуса
    /// </summary>
    public long StateId { get; set; }
}
