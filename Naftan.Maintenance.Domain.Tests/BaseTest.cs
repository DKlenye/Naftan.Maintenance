using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Naftan.Common.Domain;
using Naftan.Common.NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.IO;

namespace Naftan.Maintenance.Domain.Tests
{
    public class BaseTest
    {
        protected IUnitOfWorkFactory uowf;
        protected IRepository repository;
        protected IQueryFactory query;
        protected ISessionProvider sessionProvider;

        [SetUp]
        public void SetUp()
        {
            var container = new WindsorContainer()
                .AddFacility<TypedFactoryFacility>()
                .Install(FromAssembly.This());

            new SchemaExport(container.Resolve<INHibernateInitializer>().GetConfiguration())
               .SetOutputFile(Path.GetTempPath() + "ddl.sql")
               .Create(
                   useStdOut: true,
                   execute: true
               );

            repository = container.Resolve<IRepository>();
            sessionProvider = container.Resolve<ISessionProvider>();
            uowf = container.Resolve<IUnitOfWorkFactory>();
            query = container.Resolve<IQueryFactory>();

            using (var uow = uowf.Create())
            {
                new RepairObjectFactory(repository);
                uow.Commit();
            }

        }

    }
}
