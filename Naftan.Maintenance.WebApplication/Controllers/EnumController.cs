using System.Web.Http;
using Naftan.Common.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Naftan.Maintenance.WebApplication.Controllers
{
    public abstract class EnumController<TEnum> : ApiController
       where TEnum : struct
    {
        // GET api/<controller>
        public virtual IEnumerable Get() => Dictionary.Select(x => new { id = x.Key, name = x.Value });

        // GET api/<controller>/5
        public virtual string Get(int id) => Dictionary[id];

        private Dictionary<int, string> Dictionary => typeof(TEnum).ToDictionary();
    }
}
