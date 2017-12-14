using System.Collections.Generic;
using Naftan.Common.Domain;
using System.Linq;

namespace Naftan.Maintenance.Domain.UserReferences
{
    /// <summary>
    /// Пользовательский справочник
    /// </summary>
    public class Reference:IEntity
    {
        private readonly ICollection<ReferenceValue> values = new HashSet<ReferenceValue>();

        public int Id { get; set; }
        /// <summary>
        /// Наименование справочника
        /// </summary>
        public string Name { get; set; }

        public IEnumerable<ReferenceValue> Values => values;


        public void AddValue(ReferenceValue value)
        {
            value.Reference = this;
            values.Add(value);
        }
        public void AddValue(string value)
        {
            var newValue = new ReferenceValue
            {
                Value = value
            };
            AddValue(newValue);
        }

        public ReferenceValue this[int id] => values.FirstOrDefault(x => x.Id == id);

        public void RemoveValue(int id)
        {
            RemoveValue(this[id]);
        }

        public void RemoveValue(ReferenceValue value)
        {
            if(value!=null && values.Contains(value))
            values.Remove(value);
        }

        public void ClearValues()
        {
            values.Clear();
        }

    }
}