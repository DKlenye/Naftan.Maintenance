using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Naftan.Common.NHibernate.Conventions
{
    /// <summary>
    /// По умолчанию все свойства имеют признак not null если это не строка или Nullable<> значение
    /// </summary>
    public class PropertyNotNullConvention:IPropertyConvention,IPropertyConventionAcceptance
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Not.Nullable();
        }

        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x =>
                x.Property.PropertyType != typeof (string) &&
                Nullable.GetUnderlyingType(x.Property.PropertyType) == null
                );
        }
    }
}
