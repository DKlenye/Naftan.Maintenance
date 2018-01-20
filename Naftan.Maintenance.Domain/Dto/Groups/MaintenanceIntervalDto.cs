using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using System;

namespace Naftan.Maintenance.Domain.Dto.Groups
{
    public class MaintenanceIntervalDto : EntityDto<MaintenanceInterval>
    {

        public MaintenanceIntervalDto() { }
        public MaintenanceIntervalDto(MaintenanceInterval entity) { SetEntity(entity); }

        public int GroupId { get; private set; }
        public int MaintenanceTypeId { get; set; }
        public int? MeasureUnitId { get; set; }
        public int? MinUsage { get; set; }
        public int? MaxUsage { get; set; }
        public TimePeriod? TimePeriod { get; set; }
        public int? PeriodQuantity { get; set; }
        public int QuantityInCycle { get; set; }


        public override MaintenanceInterval GetEntity(IRepository repository)
        {
            return new MaintenanceInterval(
                    repository.Get<MaintenanceType>(MaintenanceTypeId),
                    MeasureUnitId == null ? null : repository.Get<MeasureUnit>(MeasureUnitId.Value),
                    MinUsage,
                    MaxUsage,
                    QuantityInCycle,
                    TimePeriod,
                    PeriodQuantity
                );
        }

        public override void Merge(MaintenanceInterval entity, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void SetEntity(MaintenanceInterval entity)
        {
            Id = entity.Id;
            GroupId = entity.Group.Id;
            MaintenanceTypeId = entity.MaintenanceType.Id;
            MeasureUnitId = entity.MeasureUnit?.Id;
            MinUsage = entity.MinUsage;
            MaxUsage = entity.MaxUsage;
            TimePeriod = entity.TimePeriod;
            PeriodQuantity = entity.PeriodQuantity;
            QuantityInCycle = entity.QuantityInCycle;
        }
    }
}
