using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.NHibernate;

namespace Naftan.Maintenance.WebApplication.DependencyInjection.Installers
{
    public class CommandQueryInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IQueryFactory>().ImplementedBy<QueryFactory>().LifestylePerWebRequest()
            );
        }
    }
}
