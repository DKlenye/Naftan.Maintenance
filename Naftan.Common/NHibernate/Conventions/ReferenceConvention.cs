using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Naftan.Common.Domain.Attributes;

namespace Naftan.Common.NHibernate.Conventions
{
    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {

            if (Attribute.GetCustomAttributes(instance.Property.MemberInfo, false)
                .OfType<NotNullAttribute>()
                .Any()
                )
            {
                instance.Not.Nullable();
            }
        }
    }
}
