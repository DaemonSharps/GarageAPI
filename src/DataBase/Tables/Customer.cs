using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CustomerDTO = GarageAPI.Controllers.Schemas.Customer;

namespace GarageAPI.DataBase.Tables;

/// <summary>
/// Пользователь
/// </summary>
public class Customer : CustomerDTO
{
    /// <summary>
    /// Статус пользователя
    /// </summary>
    public CustomerState CustomerState { get; set; }

    /// <summary>
    /// Записи пользователя
    /// </summary>
    public List<Record> Records { get; set; } = new List<Record>();
}
