using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Common.Domain;
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
        
        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Наименование группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тех. характеристики группы
        /// </summary>
        [JsonIgnore]
        public IEnumerable<GroupSpecification> Specifications => specifications;

        /// <summary>
        /// Межремонтные интервалы группы
        /// </summary>
        [JsonIgnore]
        public IEnumerable<MaintenanceInterval> Intervals => intervals;        

        /// <summary>
        /// Добавить характеристику
        /// </summary>
        /// <param name="specification"></param>
        public void AddSpecification(GroupSpecification specification)
        {
            if (specifications.All(f=>f.Specification.Id!=specification.Id))
            {
                specification.Group = this;
                specifications.Add(specification);
            }
        }

        /// <summary>
        /// Удалить характеристику
        /// </summary>
        /// <param name="specification"></param>
        public void RemoveSpecification(GroupSpecification specification)
        {
            if(specification!=null && specifications.Contains(specification))
            {
                specifications.Remove(specification);
            }
        }

        /// <summary>
        /// Добавить интервал
        /// </summary>
        /// <param name="intervals"></param>
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

        /// <summary>
        /// Удалить интервал
        /// </summary>
        /// <param name="interval"></param>
        public void RemoveInterval(MaintenanceInterval interval)
        {
            if(interval!=null && intervals.Contains(interval))
            {
                intervals.Remove(interval);
            }
        }

        
    }
}