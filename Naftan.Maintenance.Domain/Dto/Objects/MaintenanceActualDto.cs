using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using System;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class MaintenanceActualDto : EntityDto<MaintenanceActual>
    {

        public MaintenanceActualDto() { }
        public MaintenanceActualDto(MaintenanceActual entity) { SetEntity(entity); }

        public int MaintenanceTypeId{ get; set; }
        public DateTime StartMaintenance { get; set; }
        public DateTime? EndMaintenance { get; set; }
        public int? UnplannedReasonId { get; set; }

        public override MaintenanceActual GetEntity(IRepository repository)
        {
            throw new System.NotImplementedException();
        }

        public override void Merge(MaintenanceActual entity, IRepository repository)
        {
            throw new System.NotImplementedException();
        }

        public override void SetEntity(MaintenanceActual entity)
        {
            Id = entity.Id;
            MaintenanceTypeId = entity.MaintenanceType.Id;
            StartMaintenance = entity.StartMaintenance;
            EndMaintenance = entity.EndMaintenance;
            UnplannedReasonId = entity.UnplannedReason?.Id;
        }
    }
}
