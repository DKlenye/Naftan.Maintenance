using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Причина проведения обслуживания
    /// </summary>
    public class MaintenanceReason:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}