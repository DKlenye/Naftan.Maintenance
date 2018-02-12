using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.UserReferences
{
    /// <summary>
    /// Значение пользовательского справочника
    /// </summary>
    public class ReferenceValue:IEntity
    {
        /// <inheritdoc/>
        public int Id { get; set; }
        /// <summary>
        /// Справочник
        /// </summary>
        public Reference Reference { get; internal set; }
        /// <summary>
        /// Значение справочника
        /// </summary>
        public string Value { get; set; }

        public int? ReplicationId { get; private set; }
        public int? Replicationkmrk { get; private set; }
    }
}