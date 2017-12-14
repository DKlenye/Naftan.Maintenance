using System;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.Domain.Dto
{
    public class OperationalReport
    {
        private MaintenanceObject Object { get; set; }

        public MaintenanceType PlannedMaintenanceType { get; set; }
        public MaintenanceType ActualMaintenanceType { get; set; }
        public MaintenanceReason UnplannedReason { get; set; }
        public DateTime StartMaintenance { get; set; }
        public DateTime? EndMaintenance { get; set; }

        public int UsageBeforeMaintenance { get; set; }
        public int UsageAfterMaintenance{ get; set; }
       
        public MaintenanceType OfferForPlanMaintenance { get; set; }
        public MaintenanceReason ReasonForOffer { get; set; }
    }
}