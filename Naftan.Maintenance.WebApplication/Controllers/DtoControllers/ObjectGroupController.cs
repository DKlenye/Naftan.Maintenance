using System.Collections.Generic;
using System.Web.Http;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain;
using System.Linq;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.WebApplication.Dto.ObjectGroups;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class ObjectGroupController:ApiController
    {
        private readonly IQueryFactory query;
        private readonly IRepository repository;

        public ObjectGroupController(IRepository repository, IQueryFactory query)
        {
            this.query = query;
            this.repository = repository;
        }

        public IEnumerable<ObjectGroup> Get()
        {
            return query.FindObjectGroups();
        }

        public ObjectGroupDto Get(int id)
        {
            var group = repository.Get<ObjectGroup>(id);
            return EntityToDto(group);
        }

        public ObjectGroupDto Post([FromBody] ObjectGroupDto dto)
        {
            var group = new ObjectGroup
            {
                Name = dto.Name
            };

            if(dto.ParentGroupId != null)
            {
                var parent = repository.Get<ObjectGroup>(dto.ParentGroupId.Value);
                parent.AddChild(group);
                repository.Save(parent);
            }
            
            dto.Specifications.Where(x=>!x.Inherited).ToList().ForEach(s =>
            {
                group.AddSpecification(
                    new GroupSpecification(repository.Get<Specification>(s.SpecificationId))
                    {
                        DefaultValue = s.DefaultValue?.ToString()
                    }
                );
            });

            group.AddIntervals(dto.Intervals.Select(i =>  i.Entity(repository)).ToArray());

            repository.Save(group);

            return EntityToDto(group);
        }

        public ObjectGroupDto Put(int id, [FromBody] ObjectGroupDto dto)
        {
            var group = repository.Get<ObjectGroup>(id);

            if  (group.Parent?.Id != dto.ParentGroupId)
            {
                group.ClearParent();
                var parent = repository.Get<ObjectGroup>(dto.ParentGroupId.Value);
                parent.AddChild(group);
            }

            group.Name = dto.Name;

            var added = dto.Specifications.Where(x => x.Id == 0).ToList();
            var map = dto.Specifications.Where(x => x.Id != 0).ToDictionary(i => i.Id, i => i);
            var deleted = new List<GroupSpecification>();

            group.Specifications.ToList().ForEach(s =>
            {
                if (map.ContainsKey(s.Id))
                {
                    var _dto = map[s.Id];
                    s.DefaultValue = _dto.DefaultValue?.ToString();
                }
                else
                {
                    deleted.Add(s);
                }
            });

            deleted.ForEach(d => group.RemoveSpecification(d));
            added.ForEach(a => group.AddSpecification(a.GetEntity()));


            var added1 = dto.Intervals.Where(x => x.Id == 0).ToList();
            var map1 = dto.Intervals.Where(x => x.Id != 0).ToDictionary(i => i.Id, i => i);
            var deleted1 = new List<MaintenanceInterval>();

            group.Intervals.ToList().ForEach(s =>
            {
                if (map1.ContainsKey(s.Id))
                {
                    var _dto = map1[s.Id];

                    s.MaintenanceType = repository.Get<MaintenanceType>(_dto.MaintenanceTypeId);
                    s.MeasureUnit = _dto.MeasureUnitId ==null? null: repository.Get<MeasureUnit>(_dto.MeasureUnitId.Value);
                    s.MinUsage = _dto.MinUsage;
                    s.MaxUsage = _dto.MaxUsage;
                    s.TimePeriod = _dto.TimePeriod;
                    s.PeriodQuantity = _dto.PeriodQuantity;
                    s.QuantityInCycle = _dto.QuantityInCycle;
                }
                else
                {
                    deleted1.Add(s);
                }
            });

            deleted1.ForEach(d => group.RemoveInterval(d));
            added1.ForEach(a => group.AddIntervals(a.Entity(repository)));
            
            repository.Save(group);

            return EntityToDto(group);

        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }
        
        private ObjectGroupDto EntityToDto(ObjectGroup group)
        {
            var dto = new ObjectGroupDto
            {
                Id = group.Id,
                Name = group.Name,
                ParentGroupId = group.Parent?.Id,
            };

            var inherited = false;


            var _group = group;
            while (_group != null)
            {
                _group.Specifications.ToList().ForEach(s =>
                {
                    dto.Specifications.Add(
                        new ObjectGroupSpecificationDto(s.Id, _group.Id, s.Specification, s.DefaultValue, inherited )
                    );
                });

                inherited = true;
                _group = _group.Parent;
            }

            group.Intervals.ToList().ForEach(i =>
            {
                dto.Intervals.Add(new MaintenanceIntervalDto(i));
            });


            return dto;
        }

    }

}
