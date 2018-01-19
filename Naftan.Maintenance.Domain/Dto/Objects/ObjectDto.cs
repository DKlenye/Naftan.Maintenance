using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Maintenance.Domain.Usage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class ObjectDto : ObjectListDto
    {
        public ObjectDto() { }

        public ObjectDto(MaintenanceObject obj) 
        {
            SetEntity(obj);
        }

        public int? ParentId { get; set; }
        public IEnumerable<LastMaintenanceDto> LastMaintenance { get; set; }
        public IEnumerable<ObjectSpecificationDto> Specifications { get; set; }
        public IEnumerable<UsageActual> Usage { get; set; }
        public IEnumerable<MaintenanceActualDto> Maintenance { get; set; }


        public override MaintenanceObject GetEntity(IRepository repository)
        {
            var newObject = new MaintenanceObject(
               repository.Get<ObjectGroup>(GroupId),
               TechIndex,
               StartOperating,
               new Period(Period),
               LastMaintenance.Select(x => x.GetEntity(repository))
            );

            if (ParentId != null)
            {
                var parent = repository.Get<MaintenanceObject>(ParentId.Value);
                parent.AddChild(newObject);

                repository.Save(parent);
            }

            return newObject;

        }

        public override void SetEntity(MaintenanceObject entity)
        {
            base.SetEntity(entity);
            ParentId = entity.Parent?.Id;
            LastMaintenance = entity.LastMaintenance.Select(x => new LastMaintenanceDto(x));
            Specifications = GetSpecifications(entity);
            Usage = entity.Usage;
            Maintenance = entity.Maintenance.ToList().Select(x=>new MaintenanceActualDto(x));

        }

        public override void Merge(MaintenanceObject entity, IRepository repository)
        {
            base.Merge(entity, repository);

            //Если родитель был
            if (entity.Parent != null)
            {
                //Если родитель получен
                if (ParentId != null)
                {
                    //Если полученный родитель не равен тому, который был
                    if (ParentId != entity.Parent.Id)
                    {
                        var newParent = repository.Get<MaintenanceObject>(ParentId.Value);
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
                if (ParentId != null)
                {
                    var parent = repository.Get<MaintenanceObject>(ParentId.Value);
                    parent.AddChild(entity);

                    repository.Save(parent);
                }

            }

        }


        private IEnumerable<ObjectSpecificationDto> GetSpecifications(MaintenanceObject entity)
        {
            var objectSpecifications = entity.Specifications.ToDictionary(x => x.Specification.Id);

            var groupSpecifications = new Dictionary<int, GroupSpecification>();

            var _group = entity.Group;
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
                    entity.Id,
                    groupSpecifications[x].Specification,
                    objectSpecifications.Any() ?
                        (objectSpecifications.ContainsKey(x) ? objectSpecifications[x].Value : "") :
                        groupSpecifications[x].DefaultValue
                    );
            });
                                   
        }

        private void SetSpecifications(MaintenanceObject entity, IRepository repository)
        {
            var specificationsMap = entity.Specifications.ToDictionary(x => x.Specification.Id);
            Specifications.ToList().ForEach(x =>
            {
                //Если характеристика уже существует
                if (specificationsMap.ContainsKey(x.SpecificationId))
                {
                    //если значения нет, то удаляем характеристику
                    if (String.IsNullOrEmpty(x.Value.ToString()))
                    {
                        entity.RemoveSpecification(specificationsMap[x.SpecificationId]);
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
                        entity.AddSpecification(new ObjectSpecification(
                            repository.Get<Specification>(x.SpecificationId),
                            x.Value.ToString()
                            ));
                    }
                }
            });

        }

    }
}
