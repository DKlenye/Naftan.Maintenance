using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.WebApplication.Dto
{
    public class PlantDto : AbstractDto<Plant>
    {
        public PlantDto() { }

        public PlantDto(Plant entity)
        {
            SetEntity(entity);
        }

        public string Name { get; set; }
        public int DepartmentId { get; set; }

        public override Plant GetEntity(IRepository repository)
        {
            throw new System.NotImplementedException();
        }

        public override void Merge(Plant entity, IRepository repository)
        {
            entity.Id = Id;
            entity.Name = Name;
            entity.Department = repository.Get<Department>(DepartmentId);
        }

        public override void SetEntity(Plant entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            DepartmentId = entity.Department.Id;
        }
    }
}