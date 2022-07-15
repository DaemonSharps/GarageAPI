namespace GarageDataBase.DTO;

public class Record
{
    /// <summary>
    /// Id записи
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Время записи
    /// </summary>
    public string Time { get; set; }

    /// <summary>
    /// Дата записи
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Номер места
    /// </summary>
    public int PlaceNumber { get; set; }

    /// <summary>
    /// Id статуса записи
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Записавшийся пользователь
    /// </summary>
    public Customer Customer { get; set; }
}

