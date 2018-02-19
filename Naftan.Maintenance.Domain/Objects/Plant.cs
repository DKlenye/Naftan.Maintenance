using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Установка
    /// </summary>
    public class Plant : IEntity
    {
        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Наиенование установки
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Подразделение
        /// </summary>
        public Department Department { get; set; }
    }
}