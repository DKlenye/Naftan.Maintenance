using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Naftan.Common.NHibernate.Conventions
{
    /// <summary>
    /// Длина строковых свойств по умолчанию определяется как varchar(50)
    /// </summary>
    public class PropertyLengthConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(string))
            {
                instance.Length(150);
            }
        }
    }
}