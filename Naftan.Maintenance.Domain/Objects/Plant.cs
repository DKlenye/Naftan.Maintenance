using Naftan.Common.Domain;
using Newtonsoft.Json;
using System;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Установка
    /// </summary>
    public class Plant : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Department Department { get; set; }

        [Obsolete, JsonIgnore]
        public int? ReplicationKu { get; private set; }
        [Obsolete, JsonIgnore]
        public int? ReplicationKc { get; private set; }
    }
}