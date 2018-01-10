using Naftan.Maintenance.Domain.Objects;
using NUnit.Framework;
using System;
using System.Linq;

namespace Naftan.Maintenance.Domain.Tests.IntegrationTests
{
    public class MaintenanceObjectTest:BaseTest
    {
        [Test]
        public void MaintenanceObjectRepositoryAllTest()
        {
            var obj = repository.Get<MaintenanceObject>(1);

            var z = obj.Specifications;
            Console.WriteLine(z.Count());
        }
    }
}