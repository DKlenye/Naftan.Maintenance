using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Naftan.Common.NHibernate.Conventions
{
    public class ForeignKeyConstraintNameConvention
        : IHasManyConvention, IReferenceConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            var key = String.Format("{0}_{1}_FK", instance.Member.Name, instance.EntityType.Name);
            instance.Key.ForeignKey(key);
        }

        public void Apply(IManyToOneInstance instance)
        {
            var key = String.Format("{0}_{1}_FK", instance.EntityType.Name,instance.Property.PropertyType.Name);
            instance.ForeignKey(key);
        }

    }

    

}