using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Naftan.Common.Domain.Attributes;

namespace Naftan.Common.NHibernate.Conventions
{
    /// <summary>
    /// Соглашение not null по строковым свойствам. Если не указан аттрибут not null то строка может быть null
    /// </summary>
    public class StringPropertyNotNullConvention:IPropertyConvention,IPropertyConventionAcceptance
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Not.Nullable();
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x =>
                x.Property.PropertyType == typeof (string) &&
                Attribute.GetCustomAttributes(x.Property.MemberInfo,false).OfType<NotNullAttribute>().Any()
                );
        }
    }
}
