using System.Collections.Generic;
using Naftan.Maintenance.Domain.Objects;

namespace Naftan.Maintenance.Domain
{
    public interface IQueryFactory
    {
        IEnumerable<ObjectGroup> FindObjectGroups();
        IEnumerable<MaintenanceObject> FindObjects();
    }
}