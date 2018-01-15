using System.Collections.Generic;
using Naftan.Maintenance.Domain.Specifications;
using System;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.WebApplication.Dto.ObjectGroups
{

    public class GroupSpecificationDto:AbstractSpecificationDto<GroupSpecification>
    {
        public GroupSpecificationDto() { }

        public GroupSpecificationDto(int id, int objectGroupId, Specification specification, string value, bool inherited)
        {
            Id = id;
            SpecificationId = specification.Id;
            ObjectGroupId = objectGroupId;
            Inherited = inherited;
            SpecificationType = specification.Type;
            DefaultValue = SetValue(value);
        }

        public int ObjectGroupId { get; set; }
        public bool Inherited { get; set; }
        public int SpecificationId { get; set; }
        public string DefaultValue { get; set; }

        public override GroupSpecification GetEntity(IRepository repository)
        {
            return new GroupSpecification(repository.Get<Specification>(SpecificationId))
            {
                Id = Id,
                DefaultValue = GetValue(DefaultValue)
            };
        }

        public override void Merge(GroupSpecification entity,IRepository repository)
        {
            entity.Id = Id;
            entity.Specification = repository.Get<Specification>(SpecificationId);
            entity.DefaultValue = GetValue(DefaultValue);
        }

        public override void SetEntity(GroupSpecification entity)
        {
            throw new NotImplementedException();
        }
    }


    public class MaintenanceIntervalDto
    {
        public MaintenanceIntervalDto() { }
        public MaintenanceIntervalDto(MaintenanceInterval interval)
        {
            Id = interval.Id;
            MaintenanceTypeId = interval.MaintenanceType.Id;
            MeasureUnitId = interval.MeasureUnit?.Id;
            MinUsage = interval.MinUsage;
            MaxUsage = interval.MaxUsage;
            TimePeriod = interval.TimePeriod;
            PeriodQuantity = interval.PeriodQuantity;
            QuantityInCycle = interval.QuantityInCycle;
        }

        public int Id { get; set; }
        public int MaintenanceTypeId { get; set; }
        public int? MeasureUnitId { get; set; }
        public int? MinUsage { get; set; }
        public int? MaxUsage { get; set; }
        public TimePeriod? TimePeriod { get; set; }
        public int? PeriodQuantity { get; set; }
        public int QuantityInCycle { get; set; }

        public MaintenanceInterval Entity(IRepository repository)
        {
            return new MaintenanceInterval(
                    repository.Get<MaintenanceType>(MaintenanceTypeId),
                    MeasureUnitId == null ? null : repository.Get<MeasureUnit>(MeasureUnitId.Value),
                    MinUsage,
                    MaxUsage,
                    QuantityInCycle,
                    TimePeriod,
                    PeriodQuantity
                );
        }
    }


    public class GroupDto
    {

        public GroupDto()
        {
            Specifications = new List<GroupSpecificationDto>();
            Intervals = new List<MaintenanceIntervalDto>();
        }

        public int Id { get; set; }
        public int? ParentGroupId { get; set; }
        public string Name { get; set; }

        public IList<GroupSpecificationDto> Specifications { get; set; }
        public IList<MaintenanceIntervalDto> Intervals { get; set; }

    }
}