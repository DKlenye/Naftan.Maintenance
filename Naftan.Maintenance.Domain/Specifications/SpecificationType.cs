using System.ComponentModel;

namespace Naftan.Maintenance.Domain.Specifications
{
    /// <summary>
    /// Тип характеристики
    /// </summary>
    public enum SpecificationType
    {
        [Description("Да/Нет")]
        Boolean = 1,
        [Description("Строка")]
        String = 2,
        [Description("Число целое")]
        Int = 3,
        [Description("Число дробное")]
        Decimal = 4,
        [Description("Дата")]
        Date = 5,
        [Description("Справочник")]
        Reference = 6
    }
}