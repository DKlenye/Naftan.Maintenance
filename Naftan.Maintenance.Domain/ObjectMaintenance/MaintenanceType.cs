using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// Тип обслуживания
    /// </summary>
    public class MaintenanceType:IEntity
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
