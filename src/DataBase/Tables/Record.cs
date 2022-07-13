using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using RecordDTO = GarageAPI.Controllers.Schemas.Record;

namespace GarageAPI.DataBase.Tables;

/// <summary>
/// Запись
/// </summary>
public class Record : RecordDTO
{
    /// <summary>
    /// Id записи
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Записавшийся пользователь
    /// </summary>
    public CustomerTable Customer { get; set; }

    /// <summary>
    /// Статус записи
    /// </summary>
    public RecordState RecordState { get; set; }
}
