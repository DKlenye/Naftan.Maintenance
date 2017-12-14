using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.WebApplication.Controllers.CrudControllers
{
    public class ManufacturerController : CrudController<Manufacturer>
    {
        public ManufacturerController(IRepository repository) : base(repository)
        {
        }
    }
}