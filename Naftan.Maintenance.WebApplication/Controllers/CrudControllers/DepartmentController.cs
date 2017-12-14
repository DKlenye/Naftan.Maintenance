using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.WebApplication.Controllers.CrudControllers
{
    public class DepartmentController : CrudController<Department>
    {
        public DepartmentController(IRepository repository) : base(repository)
        {
        }
    }
}
