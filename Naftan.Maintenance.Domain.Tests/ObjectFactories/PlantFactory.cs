using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.Domain.Tests.RepairObjectFactories
{
    public class PlantFactory : AbstractFactory<Plant>
    {
        public PlantFactory(IRepository repository) : base(repository)
        {
        }

        public static Plant Unikreking { get; private set; }

        protected override void Build(IRepository repository)
        {
            var department = new Department() { Name = "1 Производство" };

            repository.Save(department);

            Unikreking = new Plant()
            {
                Department = department,
                Name = "Юникрекинг"
            };

            repository.Save(Unikreking);
        }
    }
}
