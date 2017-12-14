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
        public int Id { get; set; }
        public MaintenanceObject Object { get; internal set; }
        public DateTime StartUsage { get; internal set; }
        public DateTime EndUsage { get; internal set; }
        public int Usage { get; internal set; }
    }
}