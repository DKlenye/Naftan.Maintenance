using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;
using System;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Интервал обслуживания
    /// </summary>
    public class MaintenanceInterval:IEntity,IComparable<MaintenanceInterval>
    {
        protected MaintenanceInterval(){}

        public MaintenanceInterval(
            MaintenanceType maintenanceType,
            MeasureUnit measureUnit,
            int? minUsage = null,
            int? maxUsage = null,
            int quantityInCycle = 1,
            TimePeriod? timePeriod = null,
            int? periodQuantity = null
        )
        {
            MaintenanceType = maintenanceType;
            MeasureUnit = measureUnit;
            TimePeriod = timePeriod;
            PeriodQuantity = periodQuantity;
            MinUsage = minUsage;
            QuantityInCycle = quantityInCycle;
            MaxUsage = maxUsage;
        }




        public int Id { get; set; }

        public ObjectGroup Group { get; internal set; }
        public MaintenanceType MaintenanceType { get; set; }
        
        #region По наработке
        /// <summary>
        /// Единица измерения наработки
        /// </summary>
        public MeasureUnit MeasureUnit { get; set; }
        
        /// <summary>
        /// Наработка минимальная. 
        /// </summary>
        public int? MinUsage { get; set; }

        /// <summary>
        /// Наработка максимальная. Её может не быть
        /// </summary>
        public int? MaxUsage { get; set; }

        #endregion
        
        #region По времени

        public TimePeriod? TimePeriod { get; set; }

        /// <summary>
        /// Количество периодов
        /// </summary>
        public int? PeriodQuantity { get; set; }

        #endregion

        /// <summary>
        /// Количество в структуре межремонтного цикла
        /// todo может и не быть, а как тогда определить приоритет ремонтов? по наработке или по временым интервалам
        /// todo появилась идея сделать ссылку на интервал, который включается в более крупный public MaintenanceInterval IncludeInterval{get;set;}
        /// </summary>
        public int QuantityInCycle { get; set; }

        public int CompareTo(MaintenanceInterval other)
        {
            if (other == null) return 1;
            return QuantityInCycle.CompareTo(other.QuantityInCycle); 
        }
    }
}