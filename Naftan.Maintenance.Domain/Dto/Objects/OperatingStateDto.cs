using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;
using System;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class OperatingStateDto :EntityDto<ObjectOperatingState>
    {

        public OperatingStateDto() { }

        public OperatingStateDto(ObjectOperatingState entity) {
            SetEntity(entity);
        }

        public DateTime StartDate { get; set; }
        public OperatingState State { get; set; }

        public override ObjectOperatingState GetEntity(IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void Merge(ObjectOperatingState entity, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void SetEntity(ObjectOperatingState entity)
        {
            Id = entity.Id;
            StartDate = entity.StartDate;
            State = entity.State;
        }
    }
}
