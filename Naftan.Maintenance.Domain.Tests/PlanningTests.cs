using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Tests.RepairObjectFactories;
using NUnit.Framework;
using System.Linq;

namespace Naftan.Maintenance.Domain.Tests
{
    public class PlanningTests : BaseTest
    {
        private MaintenanceObject compressor => MaintenanceObjectFactory.Compressor;
        private OperationalReport report => compressor.Report;

        [Test]
        public void Planning_O()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 2000;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var plan = new MaintenancePlan()
                {
                    StartDate = new Period(201802).Start(),
                    EndDate = new Period(201802).End()
                };

                compressor.PlanningMaintenance(plan);

                repository.Save(plan);
                repository.Save(compressor);

                uow.Commit();

                Assert.NotNull(compressor.CurrentPlan);
                Assert.AreEqual(plan.Details.Count,1);
                Assert.AreEqual(plan.Details.First().MaintenanceType, MaintenanceTypeFactory.O_Repair);

            }
        }

        [Test]
        public void Planning_C()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 5000;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var plan = new MaintenancePlan()
                {
                    StartDate = new Period(201802).Start(),
                    EndDate = new Period(201802).End()
                };

                compressor.PlanningMaintenance(plan);

                repository.Save(plan);
                repository.Save(compressor);

                uow.Commit();

                Assert.NotNull(compressor.CurrentPlan);
                Assert.AreEqual(plan.Details.Count, 1);
                Assert.AreEqual(plan.Details.First().MaintenanceType, MaintenanceTypeFactory.T_Repair);

                
            }
        }

    }
}
