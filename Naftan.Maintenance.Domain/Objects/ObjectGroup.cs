using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Common.Domain;
using System;
using Newtonsoft.Json;
using Naftan.Maintenance.Domain.ObjectMaintenance;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Группа оборудования
    /// </summary>
    public class ObjectGroup:TreeNode<ObjectGroup>, IEntity
    {
        private readonly ICollection<GroupSpecification> specifications = new HashSet<GroupSpecification>();
        private readonly ICollection<MaintenanceInterval> intervals = new HashSet<MaintenanceInterval>();

        public int Id { get; set; }
        public string Name { get; set; }

        [Obsolete, JsonIgnore ]
        public int? ReplicationKvo { get; private set; }
        [Obsolete, JsonIgnore]
        public int? ReplicationKg { get; private set; }


        [JsonIgnore]
        public IEnumerable<GroupSpecification> Specifications => specifications;

        [JsonIgnore]
        public IEnumerable<MaintenanceInterval> Intervals => intervals;        

        public void AddSpecification(GroupSpecification specification)
        {
            if (specifications.All(f=>f.Specification.Id!=specification.Id))
            {
                specification.Group = this;
                specifications.Add(specification);
            }
        }

        public void RemoveSpecification(GroupSpecification specification)
        {
            if(specification!=null && specifications.Contains(specification))
            {
                specifications.Remove(specification);
            }
        }

        public void AddIntervals(params MaintenanceInterval[] intervals)
        {
            intervals.ToList().ForEach(interval =>
            {
                if (interval.Id == 0 || intervals.All(f => f.Id != interval.Id))
                {
                    interval.Group = this;
                    this.intervals.Add(interval);
                }
            });
        }

        public void RemoveInterval(MaintenanceInterval interval)
        {
            if(interval!=null && intervals.Contains(interval))
            {
                intervals.Remove(interval);
            }
        }

        
    }
}