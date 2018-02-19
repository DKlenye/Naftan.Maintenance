using Naftan.Maintenance.Domain.UserReferences;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Specifications
{
    /// <summary>
    /// Техническая характеристика
    /// </summary>
    public class Specification:IEntity
    {
        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Тип характеристики
        /// </summary>
        public SpecificationType Type { get; set; }
        /// <summary>
        /// Справочник
        /// </summary>
        public Reference Reference { get; set; }
    }
}