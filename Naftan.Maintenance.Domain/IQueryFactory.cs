using System.Collections.Generic;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Specifications;

namespace Naftan.Maintenance.Domain
{
    public interface IQueryFactory
    {
        IEnumerable<ObjectGroup> FindObjectGroups();
        IEnumerable<MaintenanceObject> FindObjects();
        Dictionary<int, Dictionary<int, string>> FindObjectSpecifications(int[] specificationId);
    }
}