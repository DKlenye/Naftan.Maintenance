using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Naftan.Common.Domain;
using Naftan.Common.NHibernate;
using NHibernate;

namespace Naftan.Maintenance.WebApplication.DependencyInjection.Installers
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<INHibernateInitializer>().ImplementedBy<NHibernateInitializer>(),
                Component.For(typeof(IRepository)).ImplementedBy(typeof(NHibernateRepository)).LifestylePerWebRequest(),
                Component.For<ISessionProvider>().ImplementedBy<PerRequestSessionProvider>().LifestylePerWebRequest(),
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(
                        x => x.Resolve<INHibernateInitializer>().GetConfiguration().BuildSessionFactory()));
        }
    }
}
