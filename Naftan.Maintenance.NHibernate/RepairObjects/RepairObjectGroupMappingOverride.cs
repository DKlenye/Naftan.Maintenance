using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.NHibernate.Mappings;
using FluentNHibernate.Automapping;

namespace Naftan.Maintenance.NHibernate.RepairObjects
{
    public class RepairObjectGroupMappingOverride:TreeNodeMappingOverride<ObjectGroup>
    {
        protected override string HierarchyTableName => "RepairObjectGroup_HIERARCHY";

        public override void Override(AutoMapping<ObjectGroup> mapping)
        {
            base.Override(mapping);

            mapping.HasMany(x => x.Specifications)
               .Access.ReadOnlyPropertyThroughCamelCaseField()
               .Cascade.AllDeleteOrphan()
               .Inverse()
               .AsSet()
               .LazyLoad()
               .BatchSize(250);
            
            mapping.HasMany(x => x.Intervals)
              .Access.ReadOnlyPropertyThroughCamelCaseField()
              .Cascade.AllDeleteOrphan()
              .Inverse()
              .AsSet()
              .LazyLoad()
              .BatchSize(250);
        }
    }
}
