using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto.Objects;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class ObjectController : ApiController
    {
        private IQueryFactory query;
        private IRepository repository;

        public ObjectController(IQueryFactory query, IRepository repository)
        {
            this.query = query;
            this.repository = repository;
        }

        public IEnumerable<ObjectListDto> Get()
        {
            return query.FindObjects();
        }

        public ObjectDto Get(int id) {
            return new ObjectDto(repository.Get<MaintenanceObject>(id));
        }

        public ObjectDto Post([FromBody] ObjectDto dto)
        {
            var newObject = dto.GetEntity(repository);
            repository.Save(newObject);

            return new ObjectDto(newObject);
        }

        public ObjectDto Put(int id, [FromBody] ObjectDto dto)
        {
            var entity = repository.Get<MaintenanceObject>(id);
            dto.Merge(entity, repository);

            repository.Save(entity);
            return new ObjectDto(entity);
        }

        

    }
}