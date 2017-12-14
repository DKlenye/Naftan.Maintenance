using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.UserReferences
{
    /// <summary>
    /// Значение пользовательского справочника
    /// </summary>
    public class ReferenceValue:IEntity
    {
        public int Id { get; set; }
        public Reference Reference { get; internal set; }
        /// <summary>
        /// Значение справочника
        /// </summary>
        public string Value { get; set; }
    }
}