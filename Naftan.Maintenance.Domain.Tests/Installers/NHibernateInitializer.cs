using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Naftan.Common.NHibernate;
using NHibernate.Cfg;
using Naftan.Maintenance.NHibernate;

namespace Naftan.Maintenance.Domain.Tests
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
                .ShowSql()
                .FormatSql()
                // !!!ВНИМАНИЕ Аккуратнее с настойками базы данных для тестов. Если указать рабочую(боевую) базу данных, то ВСЕ РАБОЧИЕ ДАННЫЕ БУДУТ УДАЛЕНЫ !!!
                .ConnectionString("data source = db2; initial catalog = maintenance_test; integrated security = SSPI;")
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
