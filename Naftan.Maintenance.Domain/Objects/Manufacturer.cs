using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Завод изготовитель
    /// </summary>
    public class Manufacturer : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}