using FluentNHibernate.Automapping.Alterations;
using Naftan.Maintenance.Domain.ObjectMaintenance;

namespace Naftan.Maintenance.NHibernate.ObjectMaintenance
{
    public class MaintenanceActualMappingOverride : IAutoMappingOverride<MaintenanceActual>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<MaintenanceActual> mapping)
        {

            mapping.HasMany(x => x.Snapshot)
              .Access.ReadOnlyPropertyThroughCamelCaseField()
              .Cascade.AllDeleteOrphan()
              .Inverse()
              .AsSet()
              .LazyLoad()
              .BatchSize(250);

        }
    }
}
