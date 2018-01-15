using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Последнее обслуживание
    /// </summary>
    public class LastMaintenance:IEntity
    {
        protected LastMaintenance() { }

        internal LastMaintenance(MaintenanceType maintenanceType, DateTime? date,int? usage = null)
        {
            MaintenanceType = maintenanceType;
            LastMaintenanceDate = date;
            UsageFromLastMaintenance = usage;
        }

        public int Id { get; set; }
        
        /// <summary>
        /// Объект обслуживания
        /// </summary>
        public MaintenanceObject Object { get; internal set; }
        
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

        /// <summary>
        /// Сбросить последнее обслуживание
        /// </summary>
        /// <param name="Date"></param>
        public void Reset(DateTime Date)
        {
            LastMaintenanceDate = Date;
            if (UsageFromLastMaintenance != null)
            {
                UsageFromLastMaintenance = 0;
            }
        }

        /// <summary>
        /// Добавить наработку
        /// </summary>
        /// <param name="Usage"></param>
        public void AddUsage(int Usage)
        {
            if (UsageFromLastMaintenance != null)
            {
                UsageFromLastMaintenance += Usage;
            }
        }
    }
}
