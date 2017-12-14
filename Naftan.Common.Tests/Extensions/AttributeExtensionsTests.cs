using System.ComponentModel;
using Naftan.Common.Extensions;
using NUnit.Framework;

namespace Naftan.Common.Tests.Extensions
{
    [System.ComponentModel.Description("IsClass")]
    public class ClassWithAttribute
    {
        [DisplayName("IsProperty")]
        public string PropertyWithAttribute { get; set; }
    }

    public class AttributeExtensionsTests
    {
        private ClassWithAttribute Object;

        [SetUp]
        public void SetUp()
        {
            Object = new ClassWithAttribute();
        }

        [Test]
        public void GetAttributeReturnFirstAttribute()
        {
           var classDisplayAttribute =  Object.GetType().GetAttribute<DisplayNameAttribute>();
           var classDescriptionAttribute = Object.GetType().GetAttribute<System.ComponentModel.DescriptionAttribute>();

           Assert.IsNull(classDisplayAttribute);
           Assert.IsNotNull(classDescriptionAttribute);
        }

        [Test]
        public void ContainsAttributeTest()
        {
            var prop = Object.GetType().GetProperty("PropertyWithAttribute");
            Assert.IsTrue(prop.HasAttribute<DisplayNameAttribute>());
            Assert.IsFalse(prop.HasAttribute<System.ComponentModel.DescriptionAttribute>());
        }


    }
}
