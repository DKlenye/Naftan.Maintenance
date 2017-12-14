using System;

namespace Naftan.Common.Domain.EntityComponents
{
    /// <summary>
    /// Отчётный период
    /// </summary>
    public class Period:IEntityComponent
    {
        [Obsolete("ORM Required")]
        protected Period() { }

        /// <summary>
        /// период в формате yyyymm
        /// </summary>
        public int period { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period">период в формате yyyymm</param>
        public Period(int period)
        {
            this.period = period;
        }
    }
}
