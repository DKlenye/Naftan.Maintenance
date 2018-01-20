using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Maintenance.Domain.Dto.Groups
{
    public class ObjectGroupDto : EntityDto<ObjectGroup>
    {

        public ObjectGroupDto() { }
        public ObjectGroupDto(ObjectGroup entity) { SetEntity(entity); }

        public int? ParentGroupId { get; set; }
        public string Name { get; set; }
        public IEnumerable<MaintenanceIntervalDto> Intervals { get; set; }

        public override ObjectGroup GetEntity(IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void Merge(ObjectGroup entity, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void SetEntity(ObjectGroup entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            ParentGroupId = entity.Parent?.Id;
            Intervals = entity.Intervals.Select(x => new MaintenanceIntervalDto(x));
        }
    }
}
