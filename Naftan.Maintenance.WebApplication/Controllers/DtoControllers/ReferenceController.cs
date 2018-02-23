using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.UserReferences;
using Naftan.Maintenance.WebApplication.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{

    public class ReferenceController : ApiController
    {
        private IRepository repository; 

        public ReferenceController(IRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/Reference
        public IEnumerable<ReferenceDto> Get()
        {
            return repository.All<Reference>().Select(x => (ReferenceDto) x);
        }

        // GET: api/Reference/5
        public ReferenceDto Get(int id)
        {
            return (ReferenceDto) repository.Get<Reference>(id);
        }

        // POST: api/Reference
        public ReferenceDto Post( [FromBody] ReferenceDto value)
        {
            var reference = value.Entity();
            repository.Save(reference);
            return (ReferenceDto) reference;
        }

        // PUT: api/Reference/5
        public ReferenceDto Put(int id,  [FromBody] ReferenceDto value)
        {

            var reference = repository.Get<Reference>(id);
            value.Merge(reference);
            repository.Save(reference);
            return  (ReferenceDto) reference;
        }

        // DELETE: api/Reference/5
        public void Delete(int id)
        {
            repository.Remove<Reference>(id);
        }
    }
    
}
