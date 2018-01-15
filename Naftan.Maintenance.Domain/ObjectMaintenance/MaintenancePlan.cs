using System;
using System.Collections.Generic;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Users;

namespace Naftan.Maintenance.Domain.ObjectMaintenance
{
    /// <summary>
    /// График ППР
    /// </summary>
    public class MaintenancePlan:IEntity
    {

        public MaintenancePlan() {
            Details = new HashSet<MaintenancePlanDetail>();
        }

        public int Id { get; set; }
        public User User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ISet<MaintenancePlanDetail> Details { get; set; }
    }
}