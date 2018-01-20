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

        /*
        [HttpGet,Route("api/object/specifications/{id}")]
        public IEnumerable<ObjectSpecificationDto> GetSpecifications(int id)
        {
            var o = repository.Get<MaintenanceObject>(id);
            var anySpecifications = o.Specifications.Any();

            var objectSpecifications = o.Specifications.ToDictionary(x => x.Specification.Id);

            var groupSpecifications = new Dictionary<int, GroupSpecification>();

            var _group = o.Group;
            while (_group != null)
            {
                _group.Specifications.ToList().ForEach(s =>
                {
                    groupSpecifications.Add(s.Specification.Id, s);
                });
                _group = _group.Parent;
            }

            //Берём тех. характеристики из группы объекта
            //Если есть своё значение тех.характеристики, то берём его, если нет, то пустая стока
            //Если у объекта характеристик нет (он был только добавлен), то устанавливаем значение по умолчанию для группы
            return groupSpecifications.Keys.Select(x =>
            {
                return new ObjectSpecificationDto(
                    id,
                    groupSpecifications[x].Specification,
                    anySpecifications ?
                        (objectSpecifications.ContainsKey(x) ? objectSpecifications[x].Value : "") :
                        groupSpecifications[x].DefaultValue
                    );
            });
        }

        [HttpPost,Route("api/object/specifications")]
        public IEnumerable<ObjectSpecificationDto> SetSpecifications(ListSerializer<ObjectSpecificationDto> list)
        {
            if (!list.data.Any()) return list.data;

            var objectId = list.data.First().ObjectId;

            var maintenanceObject = repository.Get<MaintenanceObject>(objectId);
            var specificationsMap = maintenanceObject.Specifications.ToDictionary(x => x.Specification.Id);

            list.data.ToList().ForEach(x =>
            {
                //Если характеристика уже существует
                if (specificationsMap.ContainsKey(x.SpecificationId))
                {
                    //если значения нет, то удаляем характеристику
                    if (String.IsNullOrEmpty(x.Value.ToString()))
                    {
                        maintenanceObject.RemoveSpecification(specificationsMap[x.SpecificationId]);
                    }
                    else
                    {
                        specificationsMap[x.SpecificationId].Value = x.Value.ToString();
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(x.Value?.ToString()))
                    {
                        maintenanceObject.AddSpecification(new ObjectSpecification(
                            repository.Get<Specification>(x.SpecificationId),
                            x.Value.ToString()
                            ));
                    }
                }
            });

            repository.Save(maintenanceObject);

            return list.data;
        }
        */

    }
}