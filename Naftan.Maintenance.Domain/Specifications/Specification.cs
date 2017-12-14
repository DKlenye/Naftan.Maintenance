using Naftan.Maintenance.Domain.UserReferences;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Specifications
{
    /// <summary>
    /// Техническая характеристика
    /// </summary>
    public class Specification:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SpecificationType Type { get; set; }
        public Reference Reference { get; set; }
    }
}