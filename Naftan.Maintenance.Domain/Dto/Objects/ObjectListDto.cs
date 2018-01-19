using System;
using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class ObjectListDto:EntityDto<MaintenanceObject>
    {
        public ObjectListDto() { }
        
        public ObjectListDto(MaintenanceObject obj)
        {
            SetEntity(obj);
        }

        public int GroupId { get; set; }
        public string TechIndex { get; set; }
        public int? DepartmentId { get; set; }
        public int? PlantId { get; set; }
        public int Period { get; set; }
        public OperatingState CurrentOperatingState { get; set; }
        public DateTime? StartOperating { get; private set; }

        public override MaintenanceObject GetEntity(IRepository repository)
        {
            var newObject = new MaintenanceObject(
                repository.Get<ObjectGroup>(GroupId),
                TechIndex,
                StartOperating,
                new Period(Period)
                );
            return newObject;            
        }

        public override void Merge(MaintenanceObject entity, IRepository repository)
        {
            entity.TechIndex = TechIndex;
            entity.Plant = repository.Get<Plant>(PlantId.Value);
        }

        public override void SetEntity(MaintenanceObject entity)
        {
            Id = entity.Id;
            GroupId = entity.Group.Id;
            TechIndex = entity.TechIndex;
            DepartmentId = entity.Plant?.Department?.Id;
            PlantId = entity.Plant?.Id;
            Period = entity.Report.Period.period;
            CurrentOperatingState = entity.CurrentOperatingState??OperatingState.Operating;
            StartOperating = entity.StartOperating;
        }
    }
}
