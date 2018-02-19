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

        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Объект ремонта
        /// </summary>
        public MaintenanceObject Object { get; private set; }
        /// <summary>
        /// Вид обслуживания
        /// </summary>
        public MaintenanceType MaintenanceType { get; internal set; }
        /// <summary>
        /// Начало обслуживания
        /// </summary>
        public DateTime StartMaintenance { get; internal set; }
        /// <summary>
        /// Окончание обслуживания
        /// </summary>
        public DateTime? EndMaintenance { get; internal set; }
        /// <summary>
        /// Причина внепланового обслуживания
        /// </summary>
        public MaintenanceReason UnplannedReason { get; internal set; }

        /// <summary>
        /// Снимок последнего обслуживания
        /// </summary>
        public IEnumerable<LastMaintenanceSnapshot> Snapshot => snapshot;

        /// <summary>
        /// Признак завершённости обслуживания
        /// </summary>
        /// <returns></returns>
        public bool IsFinalized() => EndMaintenance != null;
    }
}