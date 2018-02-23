using Naftan.Maintenance.Domain.UserReferences;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Maintenance.WebApplication.Dto
{

    public class ReferenceValueDto
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public static explicit operator ReferenceValueDto(ReferenceValue value)
        {
            return new ReferenceValueDto
            {
                Id = value.Id,
                Value = value.Value
            };
        }
    }

    public class ReferenceDto
    {
        public ReferenceDto()
        {
            Values = new List<ReferenceValueDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<ReferenceValueDto> Values { get; set; }


        public static explicit operator ReferenceDto(Reference reference)
        {
            return new ReferenceDto
            {
                Id = reference.Id,
                Name = reference.Name,
                Values = reference.Values.Select(v => (ReferenceValueDto)v).OrderByDescending(x => x.Id).ToList()
            };
        }


        public void Merge(Reference reference)
        {
            reference.Name = Name;

            var added = Values.Where(x => x.Id == 0).ToList();
            var map = Values.Where(x => x.Id != 0).ToDictionary(r => r.Id, r => r.Value);
            var deleted = new List<ReferenceValue>();

            reference.Values.ToList().ForEach(val =>
            {
                if (map.ContainsKey(val.Id))
                {
                    val.Value = map[val.Id];
                }
                else
                {
                    deleted.Add(val);
                }
            });

            deleted.ForEach(d => reference.RemoveValue(d));
            added.ForEach(a => reference.AddValue(a.Value));
        }

        public Reference Entity()
        {
            var reference = new Reference
            {
                Name = Name
            };

            Values.ForEach(v => reference.AddValue(v.Value));
            return reference;
        }

    }
}