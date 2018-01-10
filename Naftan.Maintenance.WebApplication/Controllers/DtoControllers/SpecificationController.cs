using Naftan.Maintenance.WebApplication.Dto;
using System.Web.Http;
using System.Collections.Generic;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Specifications;
using System.Linq;
using Naftan.Maintenance.Domain.UserReferences;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class SpecificationController : AbstractController<SpecificationDto>
    {
        public SpecificationController(IRepository repository) : base(repository)
        {
        }

        public override void Delete(int id)
        {
            repository.Remove<Specification>(id);
        }

        public override IEnumerable<SpecificationDto> Get()
        {
            return repository.All<Specification>().Select(s => (SpecificationDto)s );
        }

        public override SpecificationDto Get(int id)
        {
            return (SpecificationDto) repository.Get<Specification>(id);
        }

        public override SpecificationDto Post([FromBody] SpecificationDto entity)
        {
            var newEntity = new Specification()
            {
                Name = entity.Name,
                Reference = entity.ReferenceId==null ? null: repository.Get<Reference>(entity.ReferenceId.Value),
                Type = entity.Type
            };

            repository.Save(newEntity);

            return (SpecificationDto)newEntity;
        }

        public override SpecificationDto Put(int id, [FromBody] SpecificationDto entity)
        {
            var old = repository.Get<Specification>(id);

            old.Name = entity.Name;
            old.Reference = entity.ReferenceId == null ? null : repository.Get<Reference>(entity.ReferenceId.Value);
            old.Type = entity.Type;

            repository.Save(old);

            return (SpecificationDto)old;
        }
    }
}
