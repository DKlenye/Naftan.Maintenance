using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// График ППР
    /// </summary>
    public class MaintenancePlan:IEntity
    {
        protected MaintenancePlan() { }

        public MaintenancePlan(MaintenanceObject maintenanceObject, MaintenanceType maintenanceType, DateTime date, MaintenanceReason offer = null)
        {
            Object = maintenanceObject;
            MaintenanceType = maintenanceType;
            MaintenanceDate = date;
            OfferReason = offer;


            var map = Object.LastMaintenance.ToDictionary(x => x.MaintenanceType.Id);

            //фиксируем наработку для плана
            if (map.ContainsKey(maintenanceType.Id))
            {
                UsageForPlan = map[maintenanceType.Id].UsageFromLastMaintenance;
            }

            //Фиксируем предыдущий ремонт
            var previous = Object.Maintenance.OrderBy(x => x.StartMaintenance).LastOrDefault();

            if (previous != null)
            {
                var last = map[previous.MaintenanceType.Id];

                PreviousDate = last.LastMaintenanceDate;
                PreviousMaintenanceType = last.MaintenanceType;
                PreviousUsage = last.UsageFromLastMaintenance;
            }

        }


        public int Id { get; set; }
        [Required]
        public MaintenanceObject Object { get; private set; }

        /// <summary>
        /// Планируемая дата
        /// </summary>
        public DateTime MaintenanceDate { get; private set; }
       
        /// <summary>
        /// Причина внепланового ремонта
        /// </summary>
        public MaintenanceReason OfferReason { get; private set; }

        /// <summary>
        /// Планируемый тип обслуживания
        /// </summary>
        [Required]
        public MaintenanceType MaintenanceType { get; private set; }

        /// <summary>
        /// Наработка для плана
        /// </summary>
        public int? UsageForPlan { get; private set; }

        /// <summary>
        /// Последнее обслуживание
        /// </summary>
        public MaintenanceType PreviousMaintenanceType { get; private set; }

        /// <summary>
        /// Дата последнего обслуживания
        /// </summary>
        public DateTime? PreviousDate { get; private set; }

        /// <summary>
        /// Наработка с последнего обслуживания
        /// </summary>
        public int? PreviousUsage { get; private set; }

    }
}