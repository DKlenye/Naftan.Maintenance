using Naftan.Maintenance.Domain.Specifications;

namespace Naftan.Maintenance.WebApplication.Dto.Objects
{
    public class ObjectSpecificationDto:AbstractSpecificationDto<ObjectSpecification>
    {
        public ObjectSpecificationDto() { }

        public ObjectSpecificationDto(int objectId, Specification specification, string value)
        {
            ObjectId = objectId;
            SpecificationId = specification.Id;
            SpecificationType = specification.Type;
            Value = GetValue(SpecificationType, value);
        }

        public int ObjectId { get; set; }
        public int SpecificationId { get; set; }
        public SpecificationType SpecificationType { get; set; }
        public object Value { get; set; }

        public override ObjectSpecification GetEntity()
        {
            throw new System.NotImplementedException();
        }

        public override void Merge(ObjectSpecification entity)
        {
            throw new System.NotImplementedException();
        }

        public override void SetEntity(ObjectSpecification entity)
        {
            throw new System.NotImplementedException();
        }
    }
}