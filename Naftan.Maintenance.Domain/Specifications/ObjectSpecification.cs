using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Specifications
{
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

        public int Id { get; set; }
        public MaintenanceObject Object { get; set; }
        public Specification Specification { get; private set; }
        public string Value { get; set; }
    }
}