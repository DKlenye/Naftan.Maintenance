using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Подразделение
    /// </summary>
    public class Department:IEntity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
    }
}
