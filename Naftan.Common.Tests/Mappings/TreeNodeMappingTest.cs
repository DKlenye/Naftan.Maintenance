using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Naftan.Common.Domain;
using Naftan.Common.NHibernate.Mappings;
using Naftan.Common.NHibernate.Conventions;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using FluentNHibernate.Mapping;

namespace NHibernate.Tests.Mappings
{
    public class TreeNodeMappingTest
    {
        [Test]
        public void SaveParentWithChildTest()
        {
            var msSqlDatabase = MsSqlConfiguration.MsSql2008
                .ShowSql()
                .FormatSql()
                .ConnectionString("data source=.; initial catalog=maintenance; integrated security=SSPI;");

            var cfg = Fluently.Configure()
                .Database(msSqlDatabase)
                .Mappings(m => m.FluentMappings.Add<TestTreeClassMap>().Conventions.AddFromAssemblyOf<PrimaryKeyConvention>());
                

            var configuration = cfg.BuildConfiguration();
            var session = configuration.BuildSessionFactory().OpenSession();
            new SchemaExport(configuration).Execute(true, true, false, session.Connection, null);

            using (var tx = session.BeginTransaction())
            {
                var a = new TestTreeClass { Name = "a" };
                var b = new TestTreeClass { Name = "b" };

                a.AddChild(b);

                session.Save(b);
                session.Save(a);

                tx.Commit();
            }
        }
    }
 
    public class TestTreeClass : TreeNode<TestTreeClass>, IEntity
    {
        public virtual string Name { get; set; }
        public virtual int Id { get; set; }
    }

    public class TestTreeClassMap : ClassMap<TestTreeClass>
    {
        public TestTreeClassMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            this.TreeMap("TestTreeClass_HIERARCHY");
        }
    }


}
