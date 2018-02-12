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
        
        /// <inheritdoc/>
        public int Id { get; set; }
        
        /// <summary>
        /// Наименование справочника
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Значения справочника
        /// </summary>
        public IEnumerable<ReferenceValue> Values => values;

        /// <summary>
        /// Добавить значение
        /// </summary>
        /// <param name="value">значение справочника</param>
        public void AddValue(ReferenceValue value)
        {
            value.Reference = this;
            values.Add(value);
        }
        /// <summary>
        /// Добавить значение
        /// </summary>
        /// <param name="value">значение в виде строки</param>
        public void AddValue(string value)
        {
            var newValue = new ReferenceValue
            {
                Value = value
            };
            AddValue(newValue);
        }

        /// <summary>
        /// Индексатор для значений по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReferenceValue this[int id] => values.FirstOrDefault(x => x.Id == id);

        /// <summary>
        /// Удалить значение
        /// </summary>
        /// <param name="id">id значения</param>
        public void RemoveValue(int id)
        {
            RemoveValue(this[id]);
        }
        /// <summary>
        /// Удалить значение
        /// </summary>
        /// <param name="value">ссылка на значение</param>
        public void RemoveValue(ReferenceValue value)
        {
            if(value!=null && values.Contains(value))
            values.Remove(value);
        }

        /// <summary>
        /// Очистить справочник (удалить все значения)
        /// </summary>
        public void ClearValues()
        {
            values.Clear();
        }

    }
}