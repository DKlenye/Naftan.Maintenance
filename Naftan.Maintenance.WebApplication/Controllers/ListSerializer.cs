using System.Collections.Generic;

namespace Naftan.Maintenance.WebApplication.Controllers
{
    public class ListSerializer<T>
    {
        public IEnumerable<T> data { get; set; }
    }
}