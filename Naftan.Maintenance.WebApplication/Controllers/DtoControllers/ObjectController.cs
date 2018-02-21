using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto.Objects;
using Naftan.Maintenance.Domain.Objects;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
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
            var context = UserPrincipal.Current;
            var user = query.FindUserByLogin(context.SamAccountName);

            if (entity.IsUserHavePermission(user))
            {
                dto.Merge(entity, repository);
                repository.Save(entity);
                return new ObjectDto(entity);
            }

            throw new TypeAccessException("У вас нет доступа для редактирования данных оборудования");
        }


    }
}