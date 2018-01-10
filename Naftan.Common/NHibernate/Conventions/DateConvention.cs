using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System;

namespace Naftan.Common.NHibernate.Conventions
{
    public class DateConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Type.Name == "DateTime" || instance.Type.ToString().StartsWith("System.Nullable`1[[System.DateTime"))
            {
                instance.CustomSqlType("Date");
            }
        }
    }
}
