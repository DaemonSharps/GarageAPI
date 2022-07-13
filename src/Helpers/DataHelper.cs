using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GarageAPI.Helpers
{
    /// <summary>
    /// Помошник в работе с данными
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Проверка на дефолтное значение
        /// </summary>
        /// <typeparam name="T">Структура</typeparam>
        /// <param name="value">Переменная</param>
        /// <returns>True если value имеет дефолтное значение, иначе - false</returns>
        public static bool IsDefault<T>(this T value) where T : struct
        {
            object obj = Activator.CreateInstance(value.GetType());
            return obj.Equals(value);
        }
    }
}
