using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.WebApplication.Dto.Objects
{
    public class ObjectListDto : AbstractDto<MaintenanceObject>
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

        public override MaintenanceObject GetEntity(IRepository repository)
        {
            throw new System.NotImplementedException();
        }

        public override void Merge(MaintenanceObject entity, IRepository repository)
        {
            throw new System.NotImplementedException();
        }

        public override void SetEntity(MaintenanceObject entity)
        {
            Id = entity.Id;
            GroupId = entity.Group.Id;
            TechIndex = entity.TechIndex;
            DepartmentId = entity.Plant?.Department?.Id;
            PlantId = entity.Plant?.Id;
        }
    }
}