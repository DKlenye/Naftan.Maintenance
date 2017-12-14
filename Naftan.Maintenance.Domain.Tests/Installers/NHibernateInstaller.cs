using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Naftan.Common.Domain;
using Naftan.Common.NHibernate;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Naftan.Maintenance.Domain.Tests
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<INHibernateInitializer>().ImplementedBy<NHibernateInitializer>(),
                Component.For(typeof(IRepository)).ImplementedBy(typeof(NHibernateRepository)).LifestyleSingleton(),
                Component.For<ISessionProvider>().ImplementedBy<SessionProvider>().LifestyleSingleton(),
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(
                        x => x.Resolve<INHibernateInitializer>().GetConfiguration().BuildSessionFactory()),
                        Component.For<IUnitOfWorkFactory>().ImplementedBy<NHibernateUnitOfWorkFactory>().LifestyleSingleton()
            );
            
            

        }
    }
}
