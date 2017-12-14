using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Maintenance.WebApplication.Dto.Objects;
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
            return query.FindObjects().Select(x => new ObjectListDto(x));
        }

        public ObjectDto Get(int id) {
            var maintenanceObject = repository.Get<MaintenanceObject>(id);
            return new ObjectDto(maintenanceObject);
        }

        public ObjectDto Post([FromBody] ObjectDto dto)
        {
            var newObject = new MaintenanceObject(
                repository.Get<ObjectGroup>(dto.GroupId),
                dto.TechIndex,
                DateTime.Now)
            {
                Plant = repository.Get<Plant>(dto.PlantId.Value),
                Manufacturer = dto.ManufacturerId == null ? null : repository.Get<Manufacturer>(dto.ManufacturerId.Value),
                Environment = dto.EnvironmentId == null ? null : repository.Get<Domain.Objects.Environment>(dto.EnvironmentId.Value),
                FactoryNumber = dto.FactoryNumber
            };

            if (dto.ParentId != null)
            {
                var parent = repository.Get<MaintenanceObject>(dto.ParentId.Value);
                parent.AddChild(newObject);

                repository.Save(parent);
            }
            
            repository.Save(newObject);

            return new ObjectDto(newObject);
        }

        public ObjectDto Put(int id, [FromBody] ObjectDto dto)
        {
            var entity = repository.Get<MaintenanceObject>(id);
            entity.TechIndex = dto.TechIndex;
            entity.FactoryNumber = dto.FactoryNumber;
            entity.Plant = repository.Get<Plant>(dto.PlantId.Value);
            entity.Manufacturer = dto.ManufacturerId == null ? null : repository.Get<Manufacturer>(dto.ManufacturerId.Value);
            entity.Environment = dto.EnvironmentId == null ? null : repository.Get<Domain.Objects.Environment>(dto.EnvironmentId.Value);

            //Если родитель был
            if (entity.Parent != null)
            {
                //Если родитель получен
                if (dto.ParentId != null)
                {
                    //Если полученный родитель не равен тому, который был
                    if (dto.ParentId != entity.Parent.Id)
                    {
                        var newParent = repository.Get<MaintenanceObject>(dto.ParentId.Value);
                        entity.ClearParent();
                        newParent.AddChild(entity);
                        repository.Save(newParent);
                    }
                }
                //Если родитель был, но его убрали
                else
                {
                    entity.ClearParent();
                }
            }
            //Если родителя небыло
            else
            {
                //Если родитель был установлен
                if (dto.ParentId != null)
                {
                    var parent = repository.Get<MaintenanceObject>(dto.ParentId.Value);
                    parent.AddChild(entity);

                    repository.Save(parent);
                }
                        
            }

            repository.Save(entity);

            return new ObjectDto(entity);
        }


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

    }
}