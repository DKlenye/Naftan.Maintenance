using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.WebApplication.Dto.Objects;
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

            Dictionary<int, Dictionary<int, string>> dictionary = new Dictionary<int, Dictionary<int, string>>();

            query.FindObjectSpecifications(list.data.ToArray()).ToList().ForEach(x =>
            {
                var specs = new Dictionary<int, string>();
                if (dictionary.ContainsKey(x.Object.Id))
                {
                    specs = dictionary[x.Object.Id];
                }
                else
                {
                    dictionary.Add(x.Object.Id, specs);
                }

                specs.Add(x.Specification.Id, x.Value);
                
            });

            return dictionary;

        }
    }
}
