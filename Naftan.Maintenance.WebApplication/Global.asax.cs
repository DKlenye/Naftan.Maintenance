using System.Web.Http;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Naftan.Maintenance.WebApplication.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Naftan.Maintenance.WebApplication
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var serializer = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Converters.Add(new IsoDateTimeConverter { DateTimeFormat =  "dd.MM.yyyy" });

            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(container.Kernel);

            log4net.Config.XmlConfigurator.Configure();

        }
    }
}
