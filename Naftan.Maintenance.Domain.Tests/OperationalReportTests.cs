using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Tests.RepairObjectFactories;
using NUnit.Framework;
using System.Linq;

namespace Naftan.Maintenance.Domain.Tests
{
    public class OperationalReportTests : BaseTest
    {

        private MaintenanceObject compressor => MaintenanceObjectFactory.Compressor;
        private OperationalReport report => compressor.Report;

        [Test]
        public void AddUsageFromReport()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 250;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var _compressor = repository.Get<MaintenanceObject>(compressor.Id);

                Assert.AreEqual(_compressor.Usage.Count(), 1);
                Assert.AreEqual(_compressor.UsageFromStartup, 250);

                uow.Commit();
            }
        }

        [Test]
        public void AddNotFinalizeMaintenanceFromReport()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 200;
                report.ActualMaintenanceType = MaintenanceTypeFactory.C_Repair;
                report.StartMaintenance = Period.Now().Start().AddDays(10);
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var _compressor = repository.Get<MaintenanceObject>(compressor.Id);

                Assert.AreEqual(_compressor.CurrentOperatingState, OperatingState.Maintenance);
                Assert.IsNotNull(_compressor.CurrentMaintenance);
                Assert.NotZero(_compressor.Maintenance.Count());
                Assert.Zero(_compressor.LastMaintenance.Count());
                Assert.Zero(_compressor.Report.UsageBeforeMaintenance);

                Assert.IsNotNull(_compressor.Report.ActualMaintenanceType);
                Assert.IsNotNull(_compressor.Report.StartMaintenance);

                uow.Commit();
            }
        }

        [Test]
        public void AddFinalizeMaintenanceFromReport()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 200;
                report.ActualMaintenanceType = MaintenanceTypeFactory.C_Repair;
                report.StartMaintenance = Period.Now().Start().AddDays(10);
                report.EndMaintenance = Period.Now().Start().AddDays(12);
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var _compressor = repository.Get<MaintenanceObject>(compressor.Id);

                Assert.AreEqual(_compressor.CurrentOperatingState, OperatingState.Operating);
                Assert.IsNull(_compressor.CurrentMaintenance);
                Assert.NotZero(_compressor.LastMaintenance.Count());

                uow.Commit();
            }
        }

        [Test]
        public void AddUsageAfterMaintenanceFromReport()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 250;
                report.ActualMaintenanceType = MaintenanceTypeFactory.O_Repair;
                report.StartMaintenance = Period.Now().Start().AddDays(10);
                report.EndMaintenance = Period.Now().Start().AddDays(12);
                report.UsageAfterMaintenance = 230;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var _compressor = repository.Get<MaintenanceObject>(compressor.Id);

                Assert.AreEqual(_compressor.UsageFromStartup, 480);
                Assert.AreEqual(_compressor.LastMaintenance.Where(x => x.MaintenanceType == MaintenanceTypeFactory.O_Repair).Single().UsageFromLastMaintenance, 230);
               
                uow.Commit();
            }
        }

        [Test]
        public void ApplyReportWithWriteOffObject()
        {

            var period = report.Period;
            using (var uow = uowf.Create())
            {
                report.State=OperatingState.WriteOff;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var _compressor = repository.Get<MaintenanceObject>(compressor.Id);

                Assert.AreEqual(_compressor.Report.Period,period);
                Assert.AreEqual(_compressor.CurrentOperatingState, OperatingState.WriteOff);
                    
                uow.Commit();
            }

        }

        [Test]
        public void ApplyReportToWriteOffObject()
        {

            var period = report.Period;
            using (var uow = uowf.Create())
            {
                report.State = OperatingState.WriteOff;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }


            using (var uow = uowf.Create())
            {
                report.State = OperatingState.WriteOff;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }
            
            using (var uow = uowf.Create())
            {
                var _compressor = repository.Get<MaintenanceObject>(compressor.Id);

                Assert.AreEqual(_compressor.Report.Period, period);
                Assert.AreEqual(_compressor.CurrentOperatingState, OperatingState.WriteOff);
                Assert.AreEqual(_compressor.OperatingStates.Count(), 2);

                uow.Commit();
            }

        }

        [Test]
        public void AddOfferForPlanFromReport()
        {
            using (var uow = uowf.Create())
            {
                report.UsageBeforeMaintenance = 720;
                report.OfferForPlan = MaintenanceTypeFactory.C_Repair;
                report.ReasonForOffer = MaintenanceReasonFactory.Corrosion;
                compressor.ApplyReport();

                repository.Save(compressor);
                uow.Commit();
            }

            using (var uow = uowf.Create())
            {
                var _compressor = repository.Get<MaintenanceObject>(compressor.Id);

                uow.Commit();
            }
        }





    }
}
