using Naftan.Common.Domain;
using System.Collections.Generic;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers
{
    public abstract class CrudController<TEntity> : AbstractController<TEntity>
        where TEntity:IEntity
    {
        public CrudController(IRepository repository) : base(repository)
        {
        }

        // GET api/<controller>
        public override IEnumerable<TEntity> Get()
        {
            return repository.All<TEntity>();
        }

        // GET api/<controller>/5
        public override TEntity Get(int id)
        {
            return repository.Get<TEntity>(id);
        }

        // POST api/<controller>
        public override TEntity Post([FromBody]TEntity entity)
        {
            repository.Save(entity);
            return entity;
        }

        // PUT api/<controller>/5
        public override TEntity Put(int id, [FromBody]TEntity entity)
        {
            repository.Save(entity);
            return entity;
        }

        // DELETE api/<controller>/5
        public override void Delete(int id)
        {
            repository.Remove<TEntity>(id);
        }
    }
}