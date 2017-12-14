using Naftan.Maintenance.Domain;
using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Tests
{
    public class MeasureUnitFactory:AbstractFactory<MeasureUnit>
    {
        public MeasureUnitFactory(IRepository repository) : base(repository)
        {
        }

        public static MeasureUnit WorkHours { get; private set; }
        public static MeasureUnit Km { get; private set; }
        public static MeasureUnit AirPressure { get; private set; }

        protected override void Build(IRepository repository)
        {
            WorkHours = new MeasureUnit()
            {
                Name = "Время",
                Description = "Часы наработки",
                Designation = "ч"

            };

            Km = new MeasureUnit
            {
                Name = "Расстояние",
                Description = "Пробег",
                Designation = "км"
            };

            AirPressure = new MeasureUnit
            {
                Name = "Давление",
                Description = "Воздушное давление",
                Designation = "кгс/см2"
            };

            repository.Save(WorkHours);
            repository.Save(Km);
            repository.Save(AirPressure);

        }
    }
}
