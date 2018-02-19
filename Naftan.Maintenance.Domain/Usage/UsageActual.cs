using System;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Usage
{
    /// <summary>
    /// Журнал наработки
    /// </summary>
    public class UsageActual:IEntity
    {
        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Объект ремонта
        /// </summary>
        public MaintenanceObject Object { get; internal set; }
        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime StartUsage { get; internal set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime EndUsage { get; internal set; }
        /// <summary>
        /// Наработка
        /// </summary>
        public int Usage { get; internal set; }
    }
}