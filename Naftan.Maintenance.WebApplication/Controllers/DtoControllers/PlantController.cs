using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.WebApplication.Dto;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class PlantController : AbstractController<PlantDto>
    {
        public PlantController(IRepository repository) : base(repository)
        {
        }

        public override void Delete(int id)
        {
            repository.Remove<Plant>(id);
        }

        public override IEnumerable<PlantDto> Get()
        {
            return repository.All<Plant>().Select(x => new PlantDto(x));
        }

        public override PlantDto Get(int id)
        {
            return new PlantDto(repository.Get<Plant>(id));
        }

        public override PlantDto Post([FromBody] PlantDto dto)
        {
            var entity = new Plant();
            dto.Merge(entity, repository);
            repository.Save(entity);
            return new PlantDto(entity);

        }

        public override PlantDto Put(int id, [FromBody] PlantDto dto)
        {
            var entity = repository.Get<Plant>(id);
            dto.Merge(entity, repository);
            repository.Save(entity);
            return new PlantDto(entity);
        }
    }
}
