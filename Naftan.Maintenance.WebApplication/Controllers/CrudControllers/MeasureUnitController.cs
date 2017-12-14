using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;

namespace Naftan.Maintenance.WebApplication.Controllers.CrudControllers
{
    public class MeasureUnitController : CrudController<MeasureUnit>
    {
        public MeasureUnitController(IRepository repository) : base(repository)
        {
        }
    }
}
