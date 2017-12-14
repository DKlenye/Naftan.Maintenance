using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Naftan.Common.Domain;

namespace Naftan.Common.NHibernate.Mappings
{
    public abstract class TreeNodeMappingOverride<T>:IAutoMappingOverride<T>
        where T : TreeNode<T>
    {
        public virtual void Override(AutoMapping<T> mapping)
        {
            mapping.References(x => x.Parent)
                   .LazyLoad()
                   .Nullable()
                   .Column("PARENT_ID");

            mapping.HasMany(x => x.Children)
                .Access.ReadOnlyPropertyThroughCamelCaseField()
                .Cascade.All()
                .Inverse()
                .AsSet()
                .LazyLoad()
                .BatchSize(250)
                .KeyColumn("PARENT_ID");

            mapping.HasManyToMany(x => x.Ancestors)
                .Access.ReadOnlyPropertyThroughCamelCaseField()
                .Cascade.None()
                .AsSet()
                .LazyLoad()
                .BatchSize(250)
                .Table(HierarchyTableName)
                .ParentKeyColumn("CHILD_ID")
                .ChildKeyColumn("PARENT_ID")
               .ForeignKeyConstraintNames(string.Format("FK_{0}_CHILD", HierarchyTableName), null);

            mapping.HasManyToMany(x => x.Descendants)
                .Access.ReadOnlyPropertyThroughCamelCaseField()
                .Cascade.All()
                .Inverse()
                .AsSet()
                .LazyLoad()
                .BatchSize(250)
                .Table(HierarchyTableName)
                .ParentKeyColumn("PARENT_ID")
                .ChildKeyColumn("CHILD_ID")
                .ForeignKeyConstraintNames(string.Format("FK_{0}_PARENT", HierarchyTableName), null);
        }

        protected abstract string HierarchyTableName { get; }
    }
}