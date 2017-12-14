using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Naftan.Common.NHibernate.Conventions
{
    public class HasManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Inverse();
            instance.Cascade.AllDeleteOrphan();
        }
    }
}


