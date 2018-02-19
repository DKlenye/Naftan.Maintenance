using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;

namespace Naftan.Maintenance.Domain.Tests.RepairObjectFactories
{
    public class MaintenanceReasonFactory : AbstractFactory<MaintenanceReason>
    {
        public MaintenanceReasonFactory(IRepository repository) : base(repository)
        {
        }

        public static MaintenanceReason Corrosion { get; private set; }

        protected override void Build(IRepository repository)
        {
            Corrosion = new MaintenanceReason() { Name = "Коррозия", Designation = "К" };
            repository.Save(Corrosion);
        }
    }
}
