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

        [Obsolete]
        public GroupSpecification()
        {
            
        }

        public GroupSpecification(Specification specification)
        {
            Specification = specification;
        }

        public int Id { get; set; }
        public ObjectGroup Group { get; internal set; }
        public Specification Specification { get; set; }
        public string DefaultValue { get; set; }
    }
}
