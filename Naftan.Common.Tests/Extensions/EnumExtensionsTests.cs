using System.Linq;
using Naftan.Common.Extensions;
using NUnit.Framework;

namespace Naftan.Common.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        public enum TestEnum
        {
            [System.ComponentModel.Description("FirstDescription")]
            First = 1,
            [System.ComponentModel.Description("SecondDescription")]
            Second = 2,
            [System.ComponentModel.Description("ThirdDescription")]
            Third = 3
        }

        [Test]
        public void ToDescriptionMustReturnDescription()
        {
            var description = TestEnum.Second.ToDescription();
            Assert.AreEqual(description, "SecondDescription");
        }

        [Test]
        public void ToDescriptionsMustReturnArrayOfDescriptions()
        {
            var descriptions = typeof (TestEnum).ToDescriptions().ToList();
            Assert.AreEqual(descriptions.Count,3);
            Assert.AreEqual(descriptions.First(), "FirstDescription");
        }

        [Test]
        public void ToDictionaryMustReturnDictionaryOfDescriptions()
        {
            var dictionary = typeof (TestEnum).ToDictionary();
            Assert.AreEqual(dictionary.Count, 3);
            Assert.AreEqual(dictionary[3], "ThirdDescription");
        }


    }
}
