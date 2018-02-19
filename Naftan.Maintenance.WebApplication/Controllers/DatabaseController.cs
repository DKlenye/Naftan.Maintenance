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
             
        [HttpPost,Route("api/database/update")]
        public void Update()
        {
            new SchemaUpdate(initializer.GetConfiguration())
                .Execute(
                    useStdOut:false,
                    doUpdate: true
                );
        }

        [HttpPost, Route("api/database/updateScript")]
        public string UpdateScript()
        {
            string script = "";

            new SchemaUpdate(initializer.GetConfiguration())
                .Execute(
                    x => script = x,
                    doUpdate: false
                );

            return script;
        }

    }
}
