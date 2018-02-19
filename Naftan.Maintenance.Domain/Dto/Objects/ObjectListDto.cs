using System;
using Naftan.Common.Domain;
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
        public int? ParentId { get; set; }
        public int GroupId { get; set; }
        public string TechIndex { get; set; }
        public int? DepartmentId { get; set; }
        public int? PlantId { get; set; }
        public int? Site { get; set; }
        public int? Period { get; set; }
        public int? UsageFromStartup { get; set; }

        public OperatingState CurrentOperatingState { get; set; }

        public override MaintenanceObject GetEntity(IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void Merge(MaintenanceObject entity, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void SetEntity(MaintenanceObject entity)
        {
            Id = entity.Id;
            GroupId = entity.Group.Id;
            TechIndex = entity.TechIndex;
            DepartmentId = entity.Plant?.Department?.Id;
            PlantId = entity.Plant?.Id;
            Site = entity.Site;
            Period = entity.Report?.Period?.period;
            CurrentOperatingState = entity.CurrentOperatingState??OperatingState.Operating;
            UsageFromStartup = entity.UsageFromStartup;

        }
    }
}
