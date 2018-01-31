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
                report.UsageBeforeMaintenance = 2700;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

                Assert.AreEqual(compressor.Plans.Count(),1);
                Assert.AreEqual(compressor.Plans.First().MaintenanceType, MaintenanceTypeFactory.O_Repair);
        }

        [Test]
        public void Planning_T()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 5300;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            Assert.AreEqual(compressor.Plans.Count(), 1);
            Assert.AreEqual(compressor.Plans.First().MaintenanceType, MaintenanceTypeFactory.T_Repair);
        }

        [Test]
        public void Planning_K()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 70200;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            Assert.AreEqual(compressor.Plans.Count(), 1);
            Assert.AreEqual(compressor.Plans.First().MaintenanceType, MaintenanceTypeFactory.K_Repair);
        }


        [Test]
        public void PlanningWithOfferWithoutUsagePlan()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 100;
                report.OfferForPlan = MaintenanceTypeFactory.C_Repair;
                report.ReasonForOffer = MaintenanceReasonFactory.Corrosion;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            Assert.AreEqual(compressor.Plans.Count(), 1);
            Assert.AreEqual(compressor.Plans.First().MaintenanceType, MaintenanceTypeFactory.C_Repair);
        }

        [Test]
        public void PlanningWithOfferWhenUsagePlanLesser()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 2000;
                report.OfferForPlan = MaintenanceTypeFactory.C_Repair;
                report.ReasonForOffer = MaintenanceReasonFactory.Corrosion;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            Assert.AreEqual(compressor.Plans.Count(), 1);
            Assert.AreEqual(compressor.Plans.First().MaintenanceType, MaintenanceTypeFactory.C_Repair);
        }

        [Test]
        public void PlanningWithOfferWhenUsagePlanGreater()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 5300;
                report.OfferForPlan = MaintenanceTypeFactory.O_Repair;
                report.ReasonForOffer = MaintenanceReasonFactory.Corrosion;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            Assert.AreEqual(compressor.Plans.Count(), 1);
            Assert.AreEqual(compressor.Plans.First().MaintenanceType, MaintenanceTypeFactory.T_Repair);
        }



    }
}
