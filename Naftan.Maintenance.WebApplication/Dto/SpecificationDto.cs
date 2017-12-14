using Naftan.Maintenance.Domain.Specifications;


namespace Naftan.Maintenance.WebApplication.Dto
{
    public class SpecificationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SpecificationType Type { get; set; }
        public int? ReferenceId { get; set; }


        public static explicit operator SpecificationDto(Specification specification)
        {
            return new SpecificationDto
            {
                Id = specification.Id,
                Name = specification.Name,
                ReferenceId = specification.Reference?.Id,
                Type = specification.Type
            };
        }

    }
}