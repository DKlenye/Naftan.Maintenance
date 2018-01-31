using Naftan.Maintenance.Domain.Objects;
using System;

namespace Naftan.Maintenance.Domain.Dto
{
    public class OperationalReportDto
    {
        public int Id { get; set; }
        public int ObjectGroupId { get; set; }
        public string Model { get; set; }
        public string TechIndex { get; set; }
        public int DepartmentId { get; set; }
        public int PlantId { get; set; }
        public DateTime? StartMaintenance { get; set; }
        public DateTime? EndMaintenance { get; set; }
        public int UsageParent { get; set; }
        public int UsageBeforeMaintenance { get; set; }
        public int UsageAfterMaintenance { get; set; }
        public OperatingState State { get; set; }
        public int? OfferForPlan { get; set; }
        public int? PlannedMaintenanceType { get; set; }
        public bool IsTransfer { get; set; }
        public int? ActualMaintenanceType { get; set; }
        public int? UnplannedReason { get; set; }
        public int? ReasonForOffer { get; set; }
        public int Period { get; set; }
        public string NextMaintenance { get; set; }
        public int? NextUsageNorm { get; set; }
        public int? NextUsageNormMax { get; set; }
        public int? NextUsageFact { get; set; }

    }
}
