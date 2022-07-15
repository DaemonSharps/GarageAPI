﻿using System.ComponentModel.DataAnnotations;

namespace GarageDataBase.DTO;

public class Customer
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string SecondName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Почта
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Id статуса
    /// </summary>
    public string Status { get; set; }
}
