using Naftan.Common.Domain;
using System;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Последнее обслуживание, которое сохраняется перед вводом обслуживания и сбросом данных
    /// </summary>
    public class LastMaintenanceSnapshot:IEntity
    {

        protected LastMaintenanceSnapshot() { }
        public LastMaintenanceSnapshot(MaintenanceActual maintenance, LastMaintenance last) {
            Maintenance = maintenance;
            MaintenanceType = last.MaintenanceType;
            LastMaintenanceDate = last.LastMaintenanceDate;
            UsageFromLastMaintenance = last.UsageFromLastMaintenance;
        }

        public int Id { get; set; }
        /// <summary>
        /// Обслуживание
        /// </summary>
        public MaintenanceActual Maintenance { get; private set; }
        /// <summary>
        /// Вид обслуживания
        /// </summary>
        public MaintenanceType MaintenanceType { get; private set; }

        /// <summary>
        /// Дата с последнего обслуживания
        /// </summary>
        public DateTime? LastMaintenanceDate { get; private set; }

        /// <summary>
        /// Наработка с последнего обслуживания, если межремонтный интервал по времени, то наработка не учитывается
        /// </summary>
        public int? UsageFromLastMaintenance { get; private set; }

    }
}
