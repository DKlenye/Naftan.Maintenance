using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Проведённое обслуживание (Журнал обслуживания)
    /// </summary>
    public class MaintenanceActual:IEntity
    {
        public int Id { get; set; }
        public MaintenanceObject Object { get; internal set; }
        public MaintenanceType MaintenanceType { get; internal set; }
        public DateTime StartMaintenance { get; internal set; }
        public DateTime? EndMaintenance { get; internal set; }
        public MaintenanceReason UnplannedReason { get; internal set; }
 
        public bool IsFinalized() => EndMaintenance != null;
    }
}