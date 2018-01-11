﻿using FluentNHibernate.Automapping;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.NHibernate.Mappings;

namespace Naftan.Maintenance.NHibernate.RepairObjects
{
    public class RepairObjectMappingOverride:TreeNodeMappingOverride<MaintenanceObject>
    {
        protected override string HierarchyTableName => "MaintenanceObject_HIERARCHY";

        public override void Override(AutoMapping<MaintenanceObject> mapping)
        {
            base.Override(mapping);

            mapping.HasMany(x => x.OperatingStates)
                .Access.ReadOnlyPropertyThroughCamelCaseField()
                .Cascade.AllDeleteOrphan ()
                .Inverse()
                .AsSet()
                .LazyLoad()
                .BatchSize(250);

            mapping.HasMany(x => x.Specifications)
                .Access.ReadOnlyPropertyThroughCamelCaseField()
                .Cascade.AllDeleteOrphan()
                .Inverse()
                .AsSet()
                .LazyLoad()
                .BatchSize(250);

            mapping.HasMany(x => x.LastMaintenance)
               .Access.ReadOnlyPropertyThroughCamelCaseField()
               .Cascade.AllDeleteOrphan()
               .Inverse()
               .AsSet()
               .LazyLoad()
               .BatchSize(250);

            mapping.HasMany(x => x.Usage)
               .Access.ReadOnlyPropertyThroughCamelCaseField()
               .Cascade.AllDeleteOrphan()
               .Inverse()
               .AsSet()
               .LazyLoad()
               .BatchSize(250);

            mapping.HasMany(x => x.Maintenance)
              .Access.ReadOnlyPropertyThroughCamelCaseField()
              .Cascade.AllDeleteOrphan()
              .Inverse()
              .AsSet()
              .LazyLoad()
              .BatchSize(250);

            mapping.HasMany(x => x.Offers)
            .Access.ReadOnlyPropertyThroughCamelCaseField()
            .Cascade.AllDeleteOrphan()
            .Inverse()
            .AsSet()
            .LazyLoad()
            .BatchSize(250);

            mapping.IgnoreProperty(x => x.Intervals);
            mapping.HasOne(x => x.Report).PropertyRef(x => x.MaintenanceObject).Cascade.All().Fetch.Join();
        }
    }
    

}
