using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Причина проведения обслуживания
    /// </summary>
    public class MaintenanceReason:IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Обозначение
        /// </summary>
        public string Designation { get; set; }
    }
}