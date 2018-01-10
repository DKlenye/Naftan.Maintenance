using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Naftan.Common.NHibernate;
using NHibernate.Cfg;
using Naftan.Maintenance.NHibernate;
using System.IO;

namespace Naftan.Maintenance.Domain.Tests
{
    public class NHibernateInitializer:INHibernateInitializer
    {
        public Configuration GetConfiguration()
        {
            var NHibernateAssembly = Assembly.Load("Naftan.Maintenance.NHibernate");
            var DomainAssembly = Assembly.Load("Naftan.Maintenance.Domain");
            var CommonAssembly = Assembly.Load("Naftan.Common");
            
            var msSqlDatabase = MsSqlConfiguration.MsSql2012
                .UseOuterJoin()
                .ShowSql()
                .FormatSql()
                .ConnectionString("data source=.; initial catalog=maintenance; integrated security=SSPI;")
                .UseReflectionOptimizer()
                .AdoNetBatchSize(100);

            var automapping = AutoMap.Assemblies(new AutomappingConfig(), CommonAssembly, DomainAssembly)
                .Conventions.AddAssembly(CommonAssembly)
                .AddMappingsFromAssembly(NHibernateAssembly)
                .UseOverridesFromAssembly(NHibernateAssembly);

            FluentConfiguration cfg = Fluently.Configure()
                .Database(msSqlDatabase)
                .Mappings(x => x.AutoMappings.Add(automapping)
                   // .ExportTo(Path.GetTempPath()+"Mappings\\")
            );
                        
            var configuration = cfg.BuildConfiguration();
            return configuration;
        }
    }
}
