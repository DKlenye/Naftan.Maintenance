using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Предложения к плану на следующий период
    /// </summary>
    public class MaintenanceOffer:IEntity
    {
        public int Id { get; set; }
        public MaintenanceObject Object { get; set; }
        public Period Period { get; set; }
        private MaintenanceType MaintenanceType { get; set; }
        private MaintenanceReason Reason { get; set; }
    }
}