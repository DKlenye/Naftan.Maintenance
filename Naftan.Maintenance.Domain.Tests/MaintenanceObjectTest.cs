using System;
using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Usage;
using NUnit.Framework;

namespace Naftan.Maintenance.Domain.Tests
{
    public class MaintenanceObjectTest :BaseTest
    {
        //Ввод наработки объекту ремонта
        [Test]
        public void AddUsageTest()
        {

            using (var uow = uowf.Create())
            {
                RepairObjectFactory.Car.AddUsage(DateTime.Now.AddDays(-1), DateTime.Now, 100);
                repository.Save(RepairObjectFactory.Car);
                uow.Commit();
            }

            MaintenanceObject repairObject;
            IEnumerable<UsageActual> repairObjectUsage;

            using (var uow = uowf.Create())
            {
                repairObject = repository.Get<MaintenanceObject>(RepairObjectFactory.Car.Id);
                repairObjectUsage = repairObject.Usage;
                uow.Commit();
            }

            //Наработка должна суммироваться с начала эксплуатации
            Assert.Greater(repairObject.UsageFromStartup, 0);

            //Должна быть сделана запись в журнал наработки
            Assert.AreEqual(repairObjectUsage.Count(), 1);

        }


        //Ввод обслуживания с типом, которого нет в интервалах объекта ремонта приводит к ошибке
        [Test]
        public void AddMaintenanceWithNotExistsTypeShouldBeExcept()
        {
            Assert.Throws<Exception>(
                delegate
                {
                    using (var uow = uowf.Create())
                    {
                        RepairObjectFactory.Car.AddMaintenance(
                            //У автомобиля нет интервала на осмотр
                            MaintenanceTypeFactory.O_Repair,
                            DateTime.Now
                            );

                        uow.Commit();
                    }
                });
        }


        //Ввод незаконченного ремонта
        [Test]
        public void AddNotFinalizedMaintenanceTest()
        {
            using (var uow = uowf.Create())
            {
                RepairObjectFactory.Car.AddMaintenance(
                        MaintenanceTypeFactory.TO1_Repair,
                        DateTime.Now
                    );

                uow.Commit();
            }

            MaintenanceObject repairObject;
            using (var uow = uowf.Create())
            {
                repairObject = repository.Get<MaintenanceObject>(RepairObjectFactory.Car.Id);
                uow.Commit();
            }

            //Объект ремонта меняет своё состояние на "на обслуживании"
            Assert.AreEqual(repairObject.CurrentOperatingState, OperatingState.Maintenance);

            //Делается запись в журнал состояние объекта ремонта
            Assert.AreEqual(repairObject.OperatingStates.Count(),2);

            //Текущее обслуживание должно быть заполнено
            Assert.IsNotNull(repairObject.CurrentMaintenance);

        }


        //Ввод законченного ремонта
        [Test]
        public void AddFinalizeMaintenance()
        {
            using (var uow = uowf.Create())
            {
                RepairObjectFactory.Car.AddMaintenance(
                        MaintenanceTypeFactory.TO1_Repair,
                        DateTime.Now.AddDays(-10),
                        DateTime.Now
                    );
                                
                uow.Commit();
            }

            MaintenanceObject repairObject;
            using (var uow = uowf.Create())
            {
                repairObject = repository.Get<MaintenanceObject>(RepairObjectFactory.Car.Id);
                uow.Commit();
            }

            //Объект ремонта меняет своё состояние на "на обслуживании" и  возобнавляет "эксплуатируется"
            Assert.AreEqual(repairObject.CurrentOperatingState, OperatingState.Operating);

            //Количество состояние объекта 3.
            //1 "эксплуатируется". -> 2. "на обслуживании" -> 3."эксплуатируется"
            Assert.AreEqual(repairObject.OperatingStates.Count(), 3);

            //Текущий ремонт сбрасывается
            Assert.IsNull(repairObject.CurrentMaintenance);

        }
        

        [Test]
        public void LastMaintenanceTest()
        {
            using (var uow = uowf.Create())
            {
                var compressor = RepairObjectFactory.Compressor;
                var now = DateTime.Now.Date;

                var counter = 100;
                DateTime date() => now.AddDays(0 - --counter);
                

                //работа
                compressor.AddUsage(date(), date(), 111);

                //обслуживание
                compressor.AddMaintenance(
                        MaintenanceTypeFactory.O_Repair,
                        date(),
                        date()
                    );

                //работа
                compressor.AddUsage(date(), date(), 212);
                compressor.AddUsage(date(), date(), 375);

                //обслуживание
                compressor.AddMaintenance(
                        MaintenanceTypeFactory.T_Repair,
                        date(),
                        date()
                    );

                //работа
                compressor.AddUsage(date(), date(), 134);
                compressor.AddUsage(date(), date(), 212);
                compressor.AddUsage(date(), date(), 375);

                //обслуживание
                compressor.AddMaintenance(
                        MaintenanceTypeFactory.O_Repair,
                        date(),
                        date()
                    );

                uow.Commit();
                                              
            }

            MaintenanceObject repairObject;
            using (var uow = uowf.Create())
            {
                repairObject = repository.Get<MaintenanceObject>(RepairObjectFactory.Compressor.Id);
                uow.Commit();
            }
            
            var lastORepair = repairObject.LastMaintenance.FirstOrDefault(x => x.MaintenanceType == MaintenanceTypeFactory.O_Repair);
            var lastTRepair = repairObject.LastMaintenance.FirstOrDefault(x => x.MaintenanceType == MaintenanceTypeFactory.T_Repair);

            Assert.AreEqual(lastORepair.UsageFromLastMaintenance, 0);

            Assert.Greater(lastTRepair.UsageFromLastMaintenance, 0);

            Assert.Less(lastTRepair.LastMaintenanceDate, lastORepair.LastMaintenanceDate);
        }
        

        [Test]
        public void PlanningMaintenanceTest()
        {
            /*
                Для того чтобы спланировать ремонт нам необходимо знать
                1. Какова наработка на данный момент c последних ремонтов
                2. Плановая наработка
                3. 
             */
        }
    }
}
