using System;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class OperationalReportDto
    {
        public int Id { get; set; }
        public DateTime StartMaintenance { get; set; }
        public DateTime EndMaintenance { get; set; }
        public int UsageBeforeMaintenance { get; set; }
        public int UsageAfterMaintenance { get; set; }
        public int State { get; set; }
        public int OfferForPlan { get; set; }
        public int PlannedMaintenanceType { get; set; }
        public int ActualMaintenanceType { get; set; }
        public int UnplannedReason { get; set; }
        public int ReasonForOffer { get; set; }
        public int Period { get; set; }
    }
}
