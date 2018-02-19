using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Naftan.Maintenance.Domain.ObjectMaintenance;

namespace Naftan.Maintenance.NHibernate.RepairObjects
{
    public class LastMaintenanceMappingOverride : IAutoMappingOverride<LastMaintenance>
    {
        public void Override(AutoMapping<LastMaintenance> mapping)
        {
            var uniqueKeyName = "LastMaintenance_Object_MaintenanceType_key";

            mapping.References(x => x.Object).UniqueKey(uniqueKeyName);
            mapping.References(x => x.MaintenanceType).UniqueKey(uniqueKeyName);
        }
    }
}
