using System.Collections.Generic;
using Naftan.Maintenance.Domain.Specifications;
using System;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.WebApplication.Dto.ObjectGroups
{

    public class ObjectGroupSpecificationDto:AbstractSpecificationDto<GroupSpecification>
    {
        public ObjectGroupSpecificationDto() { }

        public ObjectGroupSpecificationDto(int id, int objectGroupId, Specification specification, string value, bool inherited)
        {
            Id = id;
            SpecificationId = specification.Id;
            ObjectGroupId = objectGroupId;
            Inherited = inherited;
            SpecificationType = specification.Type;
            DefaultValue = GetValue(specification.Type, value);
        }

        public int ObjectGroupId { get; set; }
        public bool Inherited { get; set; }
        public int SpecificationId { get; set; }
        public SpecificationType SpecificationType { get; set; }
        public object DefaultValue { get; set; }

        public override GroupSpecification GetEntity()
        {
            return new GroupSpecification(new Specification { Id = SpecificationId })
            {
                Id = Id,
                DefaultValue = DefaultValue?.ToString(),
            };
        }

        public override void Merge(GroupSpecification entity)
        {
            throw new NotImplementedException();
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
        public int? QuantityInCycle { get; set; }

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


    public class ObjectGroupDto
    {

        public ObjectGroupDto()
        {
            Specifications = new List<ObjectGroupSpecificationDto>();
            Intervals = new List<MaintenanceIntervalDto>();
        }

        public int Id { get; set; }
        public int? ParentGroupId { get; set; }
        public string Name { get; set; }

        public IList<ObjectGroupSpecificationDto> Specifications { get; set; }
        public IList<MaintenanceIntervalDto> Intervals { get; set; }

    }
}