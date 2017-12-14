using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Naftan.Maintenance.NHibernate;

namespace Naftan.Maintenance.Domain.Tests
{
    public class CommandQueryInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IQueryFactory>().ImplementedBy<QueryFactory>().LifestyleSingleton()
            );
        }
    }
}