using Naftan.Common.Domain.EntityComponents;
using NUnit.Framework;

namespace Naftan.Common.Tests.Domain
{
    public class PeriodTests
    {
        private Period _period = new Period(201801);

        [Test]
        public void NextPeriodTest()
        {
            Assert.AreEqual(_period.Next().period, 201802);
        }

        [Test]
        public void PrevPeriodTest()
        {
            Assert.AreEqual(_period.Prev().period, 201712);
        }

        [Test]
        public void MonthPeriodTest()
        {
            Assert.AreEqual(_period.Month(), 1);
        }

        [Test]
        public void YearPeriodTest()
        {
            Assert.AreEqual(_period.Year(), 2018);
        }

        [Test]
        public void DaysPeriodTest()
        {
            Assert.AreEqual(_period.Next().Days(), 28);
        }

        [Test]
        public void HoursPeriodTest()
        {
            Assert.AreEqual(_period.Hours(), 744);
        }

    }
}
