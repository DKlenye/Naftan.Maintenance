using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.WebApplication.Controllers.CrudControllers
{
    public class EnvironmentController : CrudController<Environment>
    {
        public EnvironmentController(IRepository repository) : base(repository)
        {
        }
    }
}
