using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain
{
    /// <summary>
    /// Единица измерения
    /// </summary>
    public class MeasureUnit:IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
       public string Name { get; set; }
        /// <summary>
        /// Обозначение
        /// </summary>
        public string Designation { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}