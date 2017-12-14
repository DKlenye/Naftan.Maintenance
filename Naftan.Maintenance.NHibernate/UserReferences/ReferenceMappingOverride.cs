using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Naftan.Maintenance.Domain.UserReferences;

namespace Naftan.Maintenance.NHibernate.UserReferences
{
    public class ReferenceMappingOverride:IAutoMappingOverride<Reference>
    {
        public void Override(AutoMapping<Reference> mapping)
        {
            mapping.HasMany(x => x.Values)
               .Access.ReadOnlyPropertyThroughCamelCaseField()
               .Cascade.AllDeleteOrphan()
               .Inverse()
               .AsSet()
               .LazyLoad()
               .BatchSize(250);
        }
    }
}
