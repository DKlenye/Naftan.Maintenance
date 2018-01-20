using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Usage;
using System;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class UsageActualDto : EntityDto<UsageActual>
    {

        public UsageActualDto() { }
        public UsageActualDto(UsageActual entity) { SetEntity(entity); }

        public DateTime StartUsage { get;  set; }
        public DateTime EndUsage { get; set; }
        public int Usage { get; set; }

        public override UsageActual GetEntity(IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void Merge(UsageActual entity, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public override void SetEntity(UsageActual entity)
        {
            Id = entity.Id;
            StartUsage = entity.StartUsage;
            EndUsage = entity.EndUsage;
            Usage = entity.Usage;
        }
    }
}
