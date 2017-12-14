using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Подразделение
    /// </summary>
    public class Department:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
