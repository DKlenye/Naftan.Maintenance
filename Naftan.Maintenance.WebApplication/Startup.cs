using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Naftan.Maintenance.WebApplication.SignalR.Startup))]

namespace Naftan.Maintenance.WebApplication.SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}