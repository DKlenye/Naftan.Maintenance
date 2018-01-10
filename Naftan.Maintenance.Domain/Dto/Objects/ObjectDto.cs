using Naftan.Maintenance.Domain.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class ObjectDto : ObjectListDto
    {
        public ObjectDto() { }

        public ObjectDto(MaintenanceObject obj) : base(obj)
        {
            ParentId = obj.Parent?.Id;
        }
        public int? ParentId { get; set; }
        public IEnumerable<LastMaintenanceDto> LastMaintenance { get; set; }
        public override void SetEntity(MaintenanceObject entity)
        {
            base.SetEntity(entity);
            LastMaintenance = entity.LastMaintenance.Select(x => new LastMaintenanceDto(x));
        }
    }
}
