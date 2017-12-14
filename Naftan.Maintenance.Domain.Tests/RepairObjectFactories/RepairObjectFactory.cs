using System;
using System.Linq;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Tests
{
    public class RepairObjectFactory:AbstractFactory<MaintenanceObject>
    {
        public RepairObjectFactory(IRepository repository) : base(repository)
        {
        }

        public static MaintenanceObject ElectroMotor { get; private set; }
        public static MaintenanceObject Compressor { get; private set; }
        public static MaintenanceObject Crane { get; private set; }
        public static MaintenanceObject Car { get; private set; }

        protected override void Build(IRepository repository)
        {
            new MaintenanceTypeFactory(repository);
            new SpecificationFactory(repository);
            new MeasureUnitFactory(repository);

            BuildElectromotor(repository);
            BuildCompressor(repository);
            BuildCrane(repository);
            BuildCar(repository);
            
        }


        private void BuildElectromotor(IRepository repository)
        {
            
            var eMotors = new ObjectGroup { Name = "Электродвигатели" };
            var eMotorType = new ObjectGroup { Name = "ЭД синхронный с к.з. ротором" };
            var eMotorModel = new ObjectGroup { Name = "1AN3R-355Z-6" };

            eMotors.AddChild(eMotorType);
            eMotorType.AddChild(eMotorModel);

            eMotors.AddSpecification(new GroupSpecification(SpecificationFactory.Power));
            eMotors.AddSpecification(new GroupSpecification(SpecificationFactory.RPM));

            var interval_T = new MaintenanceInterval(
                MaintenanceTypeFactory.T_Repair,
                MeasureUnitFactory.WorkHours,
                8000,
                8760,
                12
            );

            var interval_C = new MaintenanceInterval(
                MaintenanceTypeFactory.C_Repair,
                MeasureUnitFactory.WorkHours,
                42000,
                43800,
                2
            );

            var interval_K = new MaintenanceInterval(
                MaintenanceTypeFactory.K_Repair,
                MeasureUnitFactory.WorkHours,
                130000,
                131400,
                1
            );

            eMotors.AddIntervals(interval_T, interval_C,interval_K);

            repository.Save(eMotors);

            ElectroMotor = new MaintenanceObject(eMotorModel, "К-401В", DateTime.Now)
            {
                FactoryNumber = "3452",
                InventoryNumber = "123456678"
            };

            ElectroMotor.AddSpecificationsFromGroup();
            ElectroMotor.AddSpecification(new ObjectSpecification(SpecificationFactory.Execution, "2"));

            repository.Save(ElectroMotor);
        }

        private void BuildCompressor(IRepository repository)
        {
            var nko = new ObjectGroup { Name = "Насосно-компрессорное оборудование" };
            var kompressors = new ObjectGroup { Name = "Компрессоры" };
            var kompressorsTypes = new ObjectGroup { Name = "Поршневые" };
            var kompressorsModels = new ObjectGroup { Name = "5Г-300/15-30" };

            nko.AddChild(kompressors);
            kompressors.AddChild(kompressorsTypes);
            kompressorsTypes.AddChild(kompressorsModels);

            kompressors.AddSpecification(new GroupSpecification(SpecificationFactory.InjectionPressure));

            var O = new MaintenanceInterval(
                MaintenanceTypeFactory.O_Repair,
                MeasureUnitFactory.WorkHours,
                2630,
                3210,
                12
            );
            var T = new MaintenanceInterval(
                MaintenanceTypeFactory.T_Repair,
                MeasureUnitFactory.WorkHours,
                5260,
                6420,
                6
            );
            var C = new MaintenanceInterval(
                MaintenanceTypeFactory.C_Repair,
                MeasureUnitFactory.WorkHours,
                10520,
                12840,
                5
            );
            var K = new MaintenanceInterval(
                MaintenanceTypeFactory.K_Repair,
                MeasureUnitFactory.WorkHours,
                63120,
                77040,
                1
            );

            kompressorsTypes.AddIntervals(O,T,C,K);

            repository.Save(nko);

            Compressor = new MaintenanceObject(kompressorsModels, "КП-1", DateTime.Now);
            Compressor.AddSpecificationsFromGroup();
            Compressor.Specifications.First().Value = "11,2";

            repository.Save(Compressor);
        }

        private void BuildCrane(IRepository repository)
        {
            var gpm = new ObjectGroup { Name = "Грузоподъёмные механизмы" };
            var electroCranes = new ObjectGroup { Name = "Краны электрические" };
            var bridgeCranes = new ObjectGroup { Name = "Краны мостовые" };

            gpm.AddChild(electroCranes);
            electroCranes.AddChild(bridgeCranes);

            var fullTO = new MaintenanceInterval
            (
                MaintenanceTypeFactory.TO_Full,
                null,null,null,null,
                TimePeriod.Month,
                36
            );

            var partialTO = new MaintenanceInterval
            (
                MaintenanceTypeFactory.TO_Partial,
                null, null, null, null,
                TimePeriod.Month,
                12
            );

            var O = new MaintenanceInterval
            (
                MaintenanceTypeFactory.O_Repair,
                null, null, null, 14,
                TimePeriod.Month,
                6
            );

            var T = new MaintenanceInterval
            (
                MaintenanceTypeFactory.T_Repair,
                 null, null, null, 13,
                TimePeriod.Month,
                12
            );

            var K = new MaintenanceInterval
            (
                MaintenanceTypeFactory.K_Repair,
                null, null, null, 1,
                TimePeriod.Month,
                186
            );

            bridgeCranes.AddIntervals(fullTO, partialTO, O, T, K);
            repository.Save(gpm);

            Crane = new MaintenanceObject(bridgeCranes, "", DateTime.Now) { FactoryNumber = "13505" };
            Crane.AddSpecificationsFromGroup();
            repository.Save(Crane);
        }

        private void BuildCar(IRepository repository)
        {
            var ts = new ObjectGroup { Name = "Транспортные средства" };

            ts.AddSpecification(new GroupSpecification(SpecificationFactory.RegistrationNumber));
            ts.AddSpecification(new GroupSpecification(SpecificationFactory.EngineVolume));

            var cars = new ObjectGroup { Name = "Легковые автомобили" };
            var engine_1_8 = new ObjectGroup { Name = "Объём двигателя от 1,8 л до 3,5 л" };

            cars.AddChild(engine_1_8);
            ts.AddChild(cars);

            repository.Save(ts);
            repository.Save(cars);

            //Интервал ТО-1 10 ткм, либо 1 раз в 2 года
            var Interval_TO1 = new MaintenanceInterval(
                MaintenanceTypeFactory.TO1_Repair,
                MeasureUnitFactory.Km,
                10000, null, null,
                TimePeriod.Year, 2
            );

            //Интервал ТО-2 20 ткм
            var Interval_TO2 = new MaintenanceInterval(
                MaintenanceTypeFactory.TO2_Repair,
                MeasureUnitFactory.Km,
                20000
            );

            engine_1_8.AddIntervals(Interval_TO1, Interval_TO2);

            repository.Save(engine_1_8);

            Car = new MaintenanceObject(engine_1_8, "999", DateTime.Now);
            Car.AddSpecificationsFromGroup();
            repository.Save(Car);
        }

    }
}
