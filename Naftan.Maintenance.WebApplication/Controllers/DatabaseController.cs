using Naftan.Common.NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers
{
    public class DatabaseController : ApiController
    {
        private INHibernateInitializer initializer;

        public DatabaseController(INHibernateInitializer initializer)
        {
            this.initializer = initializer;
        }

        [Route("api/database/create")]
        public void Create()
        {
            new SchemaExport(initializer.GetConfiguration())
             .Create(
                 useStdOut: false,
                 execute: true
             );
        }

        [Route("api/database/update")]
        public void Update()
        {
            new SchemaUpdate(initializer.GetConfiguration())
                .Execute(
                    useStdOut: false,
                    doUpdate: true
                );
        }


    }
}
