using System;
using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Tests.RepairObjectFactories;
using Naftan.Maintenance.Domain.Usage;
using NUnit.Framework;

namespace Naftan.Maintenance.Domain.Tests
{
    public class MaintenanceObjectTests :BaseTest
    {
        /// <summary>
        /// Ввод наработки объекту ремонта
        /// </summary>
        [Test]
        public void AddUsageTest()
        {

            using (var uow = uowf.Create())
            {
                MaintenanceObjectFactory.Car.AddUsage(DateTime.Now.AddDays(-1), DateTime.Now, 100);
                repository.Save(MaintenanceObjectFactory.Car);
                uow.Commit();
            }

            MaintenanceObject repairObject;
            IEnumerable<UsageActual> repairObjectUsage;

            using (var uow = uowf.Create())
            {
                repairObject = repository.Get<MaintenanceObject>(MaintenanceObjectFactory.Car.Id);
                repairObjectUsage = repairObject.Usage;
                uow.Commit();
            }

            //Наработка должна суммироваться с начала эксплуатации
            Assert.Greater(repairObject.UsageFromStartup, 0);

            //Должна быть сделана запись в журнал наработки
            Assert.AreEqual(repairObjectUsage.Count(), 1);

        }

        
        /// <summary>
        /// Ввод обслуживания с типом, которого нет в интервалах объекта ремонта приводит к ошибке
        /// </summary>
        [Test]
        public void AddMaintenanceWithNotExistsTypeShouldBeExcept()
        {
            Assert.Throws<Exception>(
                delegate
                {
                    using (var uow = uowf.Create())
                    {
                        MaintenanceObjectFactory.Car.AddMaintenance(
                            //У автомобиля нет интервала на осмотр
                            MaintenanceTypeFactory.O_Repair,
                            DateTime.Now
                            );

                        uow.Commit();
                    }
                });
        }


        /// <summary>
        /// Ввод незаконченного ремонта
        /// </summary>
        [Test]
        public void AddNotFinalizedMaintenanceTest()
        {
            using (var uow = uowf.Create())
            {
                MaintenanceObjectFactory.Car.AddMaintenance(
                        MaintenanceTypeFactory.TO1_Repair,
                        DateTime.Now
                    );

                uow.Commit();
            }

            MaintenanceObject repairObject;
            using (var uow = uowf.Create())
            {
                repairObject = repository.Get<MaintenanceObject>(MaintenanceObjectFactory.Car.Id);
                uow.Commit();
            }

            //Объект ремонта меняет своё состояние на "на обслуживании"
            Assert.AreEqual(repairObject.CurrentOperatingState, OperatingState.Maintenance);

            //Делается запись в журнал состояние объекта ремонта
            Assert.AreEqual(repairObject.OperatingStates.Count(),2);

            //Текущее обслуживание должно быть заполнено
            Assert.IsNotNull(repairObject.CurrentMaintenance);

        }


        /// <summary>
        /// Ввод законченного ремонта
        /// </summary>
        [Test]
        public void AddFinalizeMaintenance()
        {
            using (var uow = uowf.Create())
            {
                MaintenanceObjectFactory.Car.AddMaintenance(
                        MaintenanceTypeFactory.TO1_Repair,
                        DateTime.Now.AddDays(-10),
                        DateTime.Now
                    );
                                
                uow.Commit();
            }

            MaintenanceObject repairObject;
            using (var uow = uowf.Create())
            {
                repairObject = repository.Get<MaintenanceObject>(MaintenanceObjectFactory.Car.Id);
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
                var compressor = MaintenanceObjectFactory.Compressor;
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
                repairObject = repository.Get<MaintenanceObject>(MaintenanceObjectFactory.Compressor.Id);
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


            /*
             1. Создать план
             2. Выбрать оборудование для планирования
             3. Проверить планировалась ли оборудование ранее (есть ли текущий план)
             4. Вызвать метод планирования у объекта на планируемый период и получить плановые работы
             5. Записать детализацию работ в план
             6. Проставить текущий план по оборудованию
             
             */



        }



    }
}
