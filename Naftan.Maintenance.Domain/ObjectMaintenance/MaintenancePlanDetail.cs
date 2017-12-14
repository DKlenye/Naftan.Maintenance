using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    public class MaintenancePlanDetail : IEntity
    {
        public int Id { get; set; }
        public MaintenancePlan Plan { get; set; }
        public MaintenanceObject Object { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public MaintenanceType MaintenanceType { get; set; }
    }
}