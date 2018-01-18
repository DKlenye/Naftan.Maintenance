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


        public Period(int period) => this.period = period;
        public Period(DateTime date) => period = date.Year * 100 + date.Month;
        
        public static Period Now() => new Period(DateTime.Now);
        
        public DateTime Start() => new DateTime(Year(), Month(), 1);
        public DateTime End() => Start().AddMonths(1).AddDays(-1);

        public int Days() => DateTime.DaysInMonth(Year(), Month());
        public int Hours() => Days() * 24;

        public int Month() => period % 100;
        public int Year() => period / 100;

        public Period Next() => new Period(Start().AddMonths(1));
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

    }
}
