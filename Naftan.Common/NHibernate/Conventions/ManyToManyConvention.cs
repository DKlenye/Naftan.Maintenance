using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Naftan.Common.NHibernate.Conventions
{
    public class ManyToManyConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {

            instance.Table(String.Format("{0}{1}s",
                instance.EntityType.Name,
                instance.ChildType.Name
            ));

            if (instance.Relationship != null)
            {
                instance.Relationship.ForeignKey(string.Format("FK_{0}_{1}", instance.TableName, instance.ChildType.Name));
                instance.Key.ForeignKey(string.Format("FK_{0}_{1}", instance.TableName, instance.EntityType.Name));
            }
            
        }
    }
}