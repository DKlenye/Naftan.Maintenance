using System;
using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.ObjectMaintenance;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Оперативный отчёт по работе оборудования
    /// </summary>
    public class OperationalReport:IEntity
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Объект 
        /// </summary>
        public MaintenanceObject MaintenanceObject { get; set; }

        /// <summary>
        /// Период отчёта
        /// </summary>
        public Period Period { get; set; }

        /// <summary>
        /// Вид обслуживания факт
        /// </summary>
        public MaintenanceType ActualMaintenanceType { get; set; }

        /// <summary>
        /// Причина внепланового ремонта
        /// </summary>
        public MaintenanceReason UnplannedReason { get; set; }
        
        /// <summary>
        /// Дата начала ремонта
        /// </summary>
        public DateTime? StartMaintenance { get; set; }

        /// <summary>
        /// Дата окончания ремонта
        /// </summary>
        public DateTime? EndMaintenance { get; set; }
        
        /// <summary>
        /// Наработка до ремонта
        /// </summary>
        public int UsageBeforeMaintenance { get; set; }
        
        /// <summary>
        /// Наработка после ремонта
        /// </summary>
        public int UsageAfterMaintenance{ get; set; }

        /// <summary>
        /// Наработка родительского оборудования
        /// </summary>
        public int UsageParent { get; set; }
       
        /// <summary>
        /// Предложения к плану на следующий период
        /// </summary>
        public MaintenanceType OfferForPlan { get; set; }

        /// <summary>
        /// Причина ремонта
        /// </summary>
        public MaintenanceReason ReasonForOffer { get; set; }
        
        /// <summary>
        /// Состояние объекта ремонта
        /// </summary>
        public OperatingState State { get; set; }

    }
}