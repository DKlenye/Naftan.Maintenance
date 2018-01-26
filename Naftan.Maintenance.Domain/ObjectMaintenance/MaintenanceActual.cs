using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Проведённое обслуживание (Журнал обслуживания)
    /// </summary>
    public class MaintenanceActual:IEntity
    {
        private readonly ICollection<LastMaintenanceSnapshot> snapshot = new HashSet<LastMaintenanceSnapshot>();

        protected MaintenanceActual() { }
        public MaintenanceActual(MaintenanceObject maintenanceObject)
        {
            maintenanceObject.LastMaintenance.ToList().ForEach(x =>
            {
                snapshot.Add(new LastMaintenanceSnapshot(this, x));
            });

            Object = maintenanceObject;
        }

        public int Id { get; set; }
        public MaintenanceObject Object { get; private set; }
        public MaintenanceType MaintenanceType { get; internal set; }
        public DateTime StartMaintenance { get; internal set; }
        public DateTime? EndMaintenance { get; internal set; }
        public MaintenanceReason UnplannedReason { get; internal set; }

        public IEnumerable<LastMaintenanceSnapshot> Snapshot => snapshot;

        public bool IsFinalized() => EndMaintenance != null;
    }
}