using Naftan.Maintenance.Domain.Tests.RepairObjectFactories;
using Naftan.Maintenance.Domain.Users;
using NUnit.Framework;

namespace Naftan.Maintenance.Domain.Tests.Users
{
    public class UserPermissionsTests:BaseTest
    {
        [Test]
        public void AddUserTest()
        {
            var user = new User("kdn", "ФИО", "1234", "mail@gmail.com");
            user.Plants.Add(PlantFactory.Unikreking);
            user.ObjectGroups.Add(MaintenanceObjectFactory.Car.Group);

            repository.Save(user);
        }

    }
}
