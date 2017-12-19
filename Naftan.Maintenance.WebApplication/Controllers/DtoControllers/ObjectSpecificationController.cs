using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class ObjectSpecificationController : ApiController
    {
        private IQueryFactory query;
        private IRepository repository;

        public ObjectSpecificationController(IQueryFactory query, IRepository repository)
        {
            this.query = query;
            this.repository = repository;
        }

        public Dictionary<int, Dictionary<int, string>> Post([FromBody] ListSerializer<int> list)
        {
            return query.FindObjectSpecifications(list.data.ToArray());
        }
    }
}
