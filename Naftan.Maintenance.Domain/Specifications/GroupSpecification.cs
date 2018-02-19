using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Specifications
{
    /// <summary>
    /// Групповая тех. характеристика
    /// </summary>
    public class GroupSpecification:IEntity
    {

        protected GroupSpecification() { }

        public GroupSpecification(Specification specification)
        {
            Specification = specification;
        }

        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Группа объекта
        /// </summary>
        public ObjectGroup Group { get; internal set; }
        /// <summary>
        /// Характеристика
        /// </summary>
        public Specification Specification { get; set; }
        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        public string DefaultValue { get; set; }
    }
}
