using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.NHibernate.RepairObjects
{
    public class OperationalReportMappingOverride : IAutoMappingOverride<OperationalReport>
    {
        public void Override(AutoMapping<OperationalReport> mapping)
        {
            mapping.References(x => x.MaintenanceObject).Unique();
            mapping.References(x => x.OfferForPlan).Column("OfferForPlan").ForeignKey("OfferPlan_FK");
            mapping.References(x => x.PlannedMaintenanceType).Column("PlannedMaintenanceType").ForeignKey("PlannedMaintenanceType_FK");
            mapping.References(x => x.UnplannedReason).Column("UnplannedReason").ForeignKey("UnplannedReason_FK");
            mapping.References(x=>x.ReasonForOffer).Column("ReasonForOffer").ForeignKey("ReasonForOffer_FK");
            mapping.References(x => x.ActualMaintenanceType).Column("ActualMaintenanceType").ForeignKey("ActualMaintenanceType_FK");
        }
    }
}
