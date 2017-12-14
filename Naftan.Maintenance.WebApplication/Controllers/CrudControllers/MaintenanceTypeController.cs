using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.WebApplication.Controllers.CrudControllers
{
    public class MaintenanceTypeController : CrudController<MaintenanceType>
    {
        public MaintenanceTypeController(IRepository repository) : base(repository)
        {
        }
    }
}