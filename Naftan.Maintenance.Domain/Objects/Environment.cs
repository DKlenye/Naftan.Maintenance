using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Среда
    /// </summary>
    public class Environment:IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Наименование среды
        /// </summary>
        public string Name { get; set; }
    }
}
