using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;

namespace Naftan.Maintenance.WebApplication.Controllers.CrudControllers
{
    public class MaintenanceReasonController : CrudController<MaintenanceReason>
    {
        public MaintenanceReasonController(IRepository repository) : base(repository)
        {
        }
    }
}
