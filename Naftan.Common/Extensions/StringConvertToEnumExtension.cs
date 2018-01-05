using System;

namespace Naftan.Common.Extensions
{
    /// <summary>
    /// Расширение для преобразования строк
    /// </summary>
    public static class StringConvertExtensions
    {
        /// <summary>
        /// Преобразования стоки в значение перечисления
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <param name="value">Строка</param>
        /// <returns>Знчение перечисления</returns>
        public static T ConvertToEnum<T>(this string value) where T : new()
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException("T must be an Enum");
            try
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            catch
            {
                throw new NotSupportedException(String.Format("value {0} is not found",value));
            }
        }
    }
}

