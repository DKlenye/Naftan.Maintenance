using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.WebApplication.Controllers.CrudControllers
{
    public class PlantController : CrudController<Plant>
    {
        public PlantController(IRepository repository) : base(repository)
        {
        }
    }
}
