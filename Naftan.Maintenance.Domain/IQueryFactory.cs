using System.Collections.Generic;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.Dto;
using Naftan.Maintenance.Domain.Dto.Groups;
using Naftan.Maintenance.Domain.Dto.Objects;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Users;

namespace Naftan.Maintenance.Domain
{
    public interface IQueryFactory
    {
        IEnumerable<ObjectGroup> FindObjectGroups();
        IEnumerable<ObjectListDto> FindObjects();
        IEnumerable<OperationalReportDto> FindOperationalReportAll();
        IEnumerable<OperationalReportDto> FindOperationalReportByParams(Period period, string userLogin);
        IEnumerable<MaintenancePlanDto> FindMaintenancePlanAll();
        IEnumerable<MaintenancePlanDto> FindMaintenancePlanByPeriod(Period period);
        IEnumerable<GroupIntervalDto> FindGroupInterval();
        IEnumerable<UsageDto> FindUsage();
        OperationalReportDto FindOperationalReportByObjectId(int objectId);
        Dictionary<int, Dictionary<int, string>> FindObjectSpecifications(int[] specificationId);
        User FindUserByLogin(string login);
    }
}