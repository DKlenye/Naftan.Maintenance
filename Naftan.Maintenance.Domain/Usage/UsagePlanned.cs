using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Usage
{
    /// <summary>
    /// Плановая наработка
    /// </summary>
    public class UsagePlanned:IEntity
    {
        public int Id { get; set; }
        public MaintenanceObject Object { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public int PeriodQuantity { get; set; }
        public int Usage { get; set; }
    }
}