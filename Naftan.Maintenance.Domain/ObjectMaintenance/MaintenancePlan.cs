using System;
using System.ComponentModel.DataAnnotations;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// График ППР
    /// </summary>
    public class MaintenancePlan:IEntity
    {

        public int Id { get; set; }
        [Required]
        public MaintenanceObject Object { get; set; }
        public DateTime MaintenanceDate { get; set; }
        [Required]
        public MaintenanceType MaintenanceType { get; set; }
        public MaintenanceReason OfferReason { get; set; }
    }
}