using System;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Состояния (история изменения рабочих состояний) объекта ремонта
    /// </summary>
    public class ObjectOperatingState:IEntity
    {
        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Объект ремонта
        /// </summary>
        public MaintenanceObject Object { get; set; }
        /// <summary>
        /// Дата изменения состояния
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Состояние
        /// </summary>
        public OperatingState State { get; set; }
    }
}