using System.Collections.Generic;
using Naftan.Maintenance.Domain.Objects;
using System.Linq;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using System;

namespace Naftan.Maintenance.WebApplication.Dto.Objects
{
    public class LastMaintenanceDto : AbstractDto<LastMaintenance>
    {
        public LastMaintenanceDto() { }

        public LastMaintenanceDto(LastMaintenance entity)
        {
            SetEntity(entity);
        }

        public int MaintenanceTypeId { get; private set; }
        public DateTime LastMaintenanceDate { get; private set; }
        public int? UsageFromLastMaintenance { get; set; }
        

        public override LastMaintenance GetEntity()
        {
            throw new System.NotImplementedException();
        }

        public override void Merge(LastMaintenance entity)
        {
            throw new System.NotImplementedException();
        }

        public override void SetEntity(LastMaintenance entity)
        {
            Id = entity.Id;
            MaintenanceTypeId = entity.MaintenanceType.Id;
            LastMaintenanceDate = entity.LastMaintenanceDate;
            UsageFromLastMaintenance = entity.UsageFromLastMaintenance;
        }
    }

    public class ObjectDto : ObjectListDto
    {
        public ObjectDto() { }
        public ObjectDto(MaintenanceObject obj):base(obj) {
            ParentId = obj.Parent?.Id;
        }
        public int? ParentId { get; set; }
        public IEnumerable<LastMaintenanceDto> LastMaintenance { get; set; }
        public override void SetEntity(MaintenanceObject entity)
        {
            base.SetEntity(entity);
            LastMaintenance = entity.LastMaintenance.Select(x =>new LastMaintenanceDto(x));
        }
    }
}