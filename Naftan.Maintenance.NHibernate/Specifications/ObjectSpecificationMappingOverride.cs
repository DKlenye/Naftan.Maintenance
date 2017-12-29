using FluentNHibernate.Automapping.Alterations;
using Naftan.Maintenance.Domain.Specifications;
using System;
using FluentNHibernate.Automapping;

namespace Naftan.Maintenance.NHibernate.Specifications
{
    public class ObjectSpecificationMappingOverride : IAutoMappingOverride<ObjectSpecification>
    {
        public void Override(AutoMapping<ObjectSpecification> mapping)
        {
            var keyName = "idx_Object_Specification";
            mapping.References(x => x.Object).UniqueKey(keyName);
            mapping.References(x => x.Specification).UniqueKey(keyName);
        }
    }
}
