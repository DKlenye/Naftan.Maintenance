using System;
using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.Domain.Dto
{
    /// <summary>
    /// Оперативный отчёт по работе оборудования
    /// </summary>
    public class OperationalReport:IEntity
    {

        public int Id { get; set; }

        /// <summary>
        /// Период отчёта
        /// </summary>
        public Period Period { get; set; }

        /// <summary>
        /// Объект 
        /// </summary>
        private MaintenanceObject Object { get; set; }
        
        /// <summary>
        /// Вид обслуживания план
        /// </summary>
        public MaintenanceType PlannedMaintenanceType { get; set; }

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
        public DateTime StartMaintenance { get; set; }

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
        public int? UsageAfterMaintenance{ get; set; }
       
        /// <summary>
        /// Предложения к плану на следующий период
        /// </summary>
        public MaintenanceType OfferForPlan { get; set; }

        /// <summary>
        /// Причина ремонта
        /// </summary>
        public MaintenanceReason ReasonForOffer { get; set; }
    }
}