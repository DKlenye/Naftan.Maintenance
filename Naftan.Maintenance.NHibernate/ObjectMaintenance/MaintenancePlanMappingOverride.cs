using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Naftan.Maintenance.Domain.ObjectMaintenance;

namespace Naftan.Maintenance.NHibernate.ObjectMaintenance
{
    public class MaintenancePlanMappingOverride : IAutoMappingOverride<MaintenancePlan>
    {
        public void Override(AutoMapping<MaintenancePlan> mapping)
        {
            mapping.References(x => x.PreviousMaintenanceType).Column("PreviousMaintenanceType").ForeignKey("PreviousMaintenanceType_FK");
        }
    }
}
