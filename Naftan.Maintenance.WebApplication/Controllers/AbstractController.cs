using Naftan.Common.Domain;
using System.Collections.Generic;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers
{
    public abstract class AbstractController<TResult>: ApiController
    {
        protected readonly IRepository repository;

        public AbstractController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET api/<controller>
        public abstract IEnumerable<TResult> Get();

        // GET api/<controller>/5
        public abstract TResult Get(int id);

        // POST api/<controller>
        public abstract TResult Post([FromBody]TResult entity);

        // PUT api/<controller>/5
        public abstract TResult Put(int id, [FromBody]TResult entity);

        // DELETE api/<controller>/5
        public abstract void Delete(int id);
    }
}