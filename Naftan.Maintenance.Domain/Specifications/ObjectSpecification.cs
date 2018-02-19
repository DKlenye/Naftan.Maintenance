using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Specifications
{
    /// <summary>
    /// Характеристика объекта
    /// </summary>
    public class ObjectSpecification:IEntity
    {
        [Obsolete]
        public ObjectSpecification()
        {
            
        }

        public ObjectSpecification(Specification specification, string value)
        {
            Specification = specification;
            Value = value;
        }

        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Объект ремонта
        /// </summary>
        public MaintenanceObject Object { get; set; }
        /// <summary>
        /// Характеристика
        /// </summary>
        public Specification Specification { get; private set; }
        /// <summary>
        /// Значение характеристики
        /// </summary>
        public string Value { get; set; }
    }
}