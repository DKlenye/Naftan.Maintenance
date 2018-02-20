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

        public MaintenancePlan(MaintenanceObject maintenanceObject, MaintenanceType maintenanceType, DateTime date, bool isTransfer = false, bool isOffer = false, MaintenanceReason offer = null)
        {
            Object = maintenanceObject;
            MaintenanceType = maintenanceType;
            MaintenanceDate = date;
            IsOffer = isOffer;
            OfferReason = offer;
            IsTransfer = isTransfer;

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
            //Если предыдущего обслуживания мы не нашли, то пытаемся взять дату предыдущего обслуживания в данных с последних ремонтов LastMaintenance
            else
            {
                var intervals = Object.Intervals.ToList();
                intervals.Sort();

                int? type = null;
                DateTime? lastDate = null;

                intervals.ForEach(x =>
                {
                    if (type != null && lastDate!=null)
                    {
                        var _date = map[x.MaintenanceType.Id].LastMaintenanceDate;

                        if (_date!=null && _date > lastDate)
                        {
                            type = x.MaintenanceType.Id;
                            lastDate = _date;
                        }
                    }
                    else
                    {
                        type = x.MaintenanceType.Id;
                        lastDate = map[type.Value].LastMaintenanceDate;
                    }

                });

                if(type!=null && lastDate != null)
                {
                    PreviousDate = map[type.Value].LastMaintenanceDate;
                    PreviousMaintenanceType = map[type.Value].MaintenanceType;
                    PreviousUsage = map[type.Value].UsageFromLastMaintenance;
                }

            }

        }

        /// <inheritdoc/>
        public int Id { get; set; }

        /// <summary>
        /// Объект 
        /// </summary>
        [Required]
        public MaintenanceObject Object { get; private set; }

        /// <summary>
        /// Планируемая дата
        /// </summary>
        public DateTime MaintenanceDate { get; private set; }
       

        /// <summary>
        /// По предложению
        /// </summary>
        public bool IsOffer { get; private set; }

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

        /// <summary>
        /// Признак переноса из предыдущего месяца
        /// </summary>
        public bool IsTransfer { get; private set; }

    }
}