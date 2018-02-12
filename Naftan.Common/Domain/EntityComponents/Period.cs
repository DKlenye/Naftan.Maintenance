using System;

namespace Naftan.Common.Domain.EntityComponents
{
    /// <summary>
    /// Отчётный период
    /// </summary>
    public class Period:IEntityComponent
    {
        protected Period() { }

        /// <summary>
        /// период в формате yyyymm
        /// </summary>
        public int period { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="period">период в формате yyyymm</param>
        public Period(int period) => this.period = period;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="date">дата</param>
        public Period(DateTime date) => period = date.Year * 100 + date.Month;
        
        /// <summary>
        /// Получить Текущий период
        /// </summary>
        /// <returns></returns>
        public static Period Now() => new Period(DateTime.Now);

        /// <summary>
        /// Получить начальную дату периода
        /// </summary>
        /// <returns>Начальная дата периода</returns>
        public DateTime Start() => new DateTime(Year(), Month(), 1);
        /// <summary>
        /// Получить конечную дату периода
        /// </summary>
        /// <returns> Конечная дата периода</returns>
        public DateTime End() => Start().AddMonths(1).AddDays(-1);

        /// <summary>
        /// Получить количество дней в периоде
        /// </summary>
        /// <returns>Кол-во дней</returns>
        public int Days() => DateTime.DaysInMonth(Year(), Month());

        /// <summary>
        /// Получить количество часов в периоде
        /// </summary>
        /// <returns>Количнство часов</returns>
        public int Hours() => Days() * 24;

        /// <summary>
        /// Получить месяц периода
        /// </summary>
        /// <returns></returns>
        public int Month() => period % 100;
        /// <summary>
        /// Получить год периода
        /// </summary>
        /// <returns></returns>
        public int Year() => period / 100;

        /// <summary>
        /// Получить следующий период
        /// </summary>
        /// <returns>следующий период</returns>
        public Period Next() => new Period(Start().AddMonths(1));
        /// <summary>
        /// Получить предыдущий период
        /// </summary>
        /// <returns>предыдущий период</returns>
        public Period Prev() => new Period(Start().AddMonths(-1));

        public override bool Equals(object obj)
        {
            if (obj is Period)
            {
                var p = obj as Period;
                return p.period.Equals(period);
            }
            else return false;
        }

        public override int GetHashCode()
        {
            return period.GetHashCode();
        }

    }
}
