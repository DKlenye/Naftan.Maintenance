using Naftan.Maintenance.Domain.Specifications;
using Naftan.Maintenance.Domain.UserReferences;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Tests
{
    public class SpecificationFactory:AbstractFactory<Reference>
    {
        public SpecificationFactory(IRepository repository) : base(repository)
        {
        }

        public static Specification Power { get; private set; }
        public static Specification RPM { get; private set; }
        public static Specification Execution { get; private set; }
        public static Specification EngineVolume { get; private set; }
        public static Specification RegistrationNumber { get; private set; }
        public static Specification InjectionPressure { get; private set; }


        protected override void Build(IRepository repository)
        {
            var execution = new Reference { Name = "Вид исполнения" };
            execution.AddValue("Открытое");
            execution.AddValue("Закрытое");
            repository.Save(execution);
            
            Power = new Specification
            {
                Name = "Мощность электодвигателя, КВт",
                Type = SpecificationType.Decimal
            };

            RPM = new Specification
            {
                Name = "Частота вращения, об/мин",
                Type = SpecificationType.Int
            };

            Execution = new Specification
            {
                Name = "Исполнение",
                Type = SpecificationType.Reference,
                Reference = execution
            };

            RegistrationNumber = new Specification
            {
                Name = "Гос. №",
                Type = SpecificationType.String
            };

            EngineVolume = new Specification
            {
                Name = "Объём двигателя, л",
                Type = SpecificationType.Decimal
            };

            InjectionPressure = new Specification
            {
                Name = "Давление нагнетания, кгс/м2",
                Type = SpecificationType.Decimal
            };
            
            repository.Save(Power);
            repository.Save(RPM);
            repository.Save(Execution);
            repository.Save(RegistrationNumber);
            repository.Save(EngineVolume);
            repository.Save(InjectionPressure);
        }
    }
}
