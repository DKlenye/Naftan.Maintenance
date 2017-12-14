using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Naftan.Common.NHibernate;
using NHibernate.Cfg;
using Naftan.Maintenance.NHibernate;

namespace Naftan.Maintenance.WebApplication.DependencyInjection
{
    public class NHibernateInitializer:INHibernateInitializer
    {
        public Configuration GetConfiguration()
        {
            var NHibernateAssembly = Assembly.Load("Naftan.Maintenance.NHibernate");
            var DomainAssembly = Assembly.Load("Naftan.Maintenance.Domain");
            var CommonAssembly = Assembly.Load("Naftan.Common");
            
            var msSqlDatabase = MsSqlConfiguration.MsSql2008
                .UseOuterJoin()
                .ConnectionString(s => s.FromConnectionStringWithKey("maintenance"))
                .UseReflectionOptimizer()
                .AdoNetBatchSize(100);

            var automapping = AutoMap.Assemblies(new AutomappingConfig(), CommonAssembly, DomainAssembly)
                .Conventions.AddAssembly(CommonAssembly)
                .AddMappingsFromAssembly(NHibernateAssembly)
                .UseOverridesFromAssembly(NHibernateAssembly);

            FluentConfiguration cfg = Fluently.Configure()
                .Database(msSqlDatabase)
                .Mappings(x => x.AutoMappings.Add(automapping)
            );
                        
            var configuration = cfg.BuildConfiguration();
            return configuration;
        }
    }
}
