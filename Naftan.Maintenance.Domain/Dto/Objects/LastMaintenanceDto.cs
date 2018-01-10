using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using System;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class LastMaintenanceDto : EntityDto<LastMaintenance>
    {
        public LastMaintenanceDto() { }

        public LastMaintenanceDto(LastMaintenance entity)
        {
            SetEntity(entity);
        }

        public int MaintenanceTypeId { get; private set; }
        public DateTime LastMaintenanceDate { get; private set; }
        public int? UsageFromLastMaintenance { get; set; }

        public override LastMaintenance GetEntity(IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void Merge(LastMaintenance entity, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void SetEntity(LastMaintenance entity)
        {
            Id = entity.Id;
            MaintenanceTypeId = entity.MaintenanceType.Id;
            LastMaintenanceDate = entity.LastMaintenanceDate;
            UsageFromLastMaintenance = entity.UsageFromLastMaintenance;
        }
    }
}
