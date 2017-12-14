using Naftan.Maintenance.Domain.Objects;
using NUnit.Framework;
using System;

namespace Naftan.Maintenance.Domain.Tests.IntegrationTests
{
    public class MaintenanceObjectGroupTest:BaseTest
    {

        [Test]
        public void MaintenanceObjectGroupRepositoryAllTest()
        {
           var z =  repository.All<ObjectGroup>();

        }


    }
}
