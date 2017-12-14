using System.ComponentModel;

namespace Naftan.Maintenance.Domain
{
    /// <summary>
    /// Временной период
    /// </summary>
    public enum TimePeriod
    {
        [Description("День (сутки)")]
        Day = 1,
        [Description("Неделя")]
        Week = 2,
        [Description("Месяц")]
        Month = 3,
        [Description("Квартал")]
        Quarter = 4,
        [Description("Полугодие")]
        HalfYear = 5,
        [Description("Год")]
        Year = 6
    }
}
