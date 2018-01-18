using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.NHibernate;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using Naftan.Maintenance.Domain.Users;
using Naftan.Maintenance.Domain.Dto.Objects;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.Dto.Groups;

namespace Naftan.Maintenance.NHibernate
{
    public class QueryFactory:IQueryFactory
    {
        private readonly ISessionProvider provider;
       
        public QueryFactory(ISessionProvider provider)
        {
            this.provider = provider;
        }

        private ISession Session => provider.CurrentSession;
  
        
        public IEnumerable<ObjectGroup> FindObjectGroups()
        {
            return Session.Query<ObjectGroup>()
                .Where(x => x.Parent == null)
                .ToArray();
        }

        public IEnumerable<ObjectListDto> FindObjects()
        {
            var sql = @"
             select 
                o.MaintenanceObjectId as Id,
	            o.ObjectGroupId as GroupId,
	            o.TechIndex,
	            p.DepartmentId,
	            o.PlantId,
                o.CurrentOperatingState,
                r.Period
            from MaintenanceObject o
            left join OperationalReport r on r.MaintenanceObjectId = o.MaintenanceObjectId
            left join Plant p on p.PlantId = o.PlantId";

            return Session.CreateSQLQuery(sql)
                .SetResultTransformer(Transformers.AliasToBean<ObjectListDto>())
                .List<ObjectListDto>();
        }

        public class rezult
        {
            public int ObjectId { get; set; }
            public int SpecificationId { get; set; }
            public string Value { get; set; }
        }

        public Dictionary<int, Dictionary<int, string>> FindObjectSpecifications(int[] specificationId)
        {
            Dictionary<int, Dictionary<int, string>> dictionary = new Dictionary<int, Dictionary<int, string>>();

            if (!specificationId.Any()) return dictionary;

            var rezult =  Session.CreateSQLQuery
                (@"
                SELECT 
	                MaintenanceObjectId AS ObjectId,
	                SpecificationId,
	                [Value]
                FROM ObjectSpecification where SpecificationId in (:id) ")
                .SetParameterList("id",specificationId)
                .SetResultTransformer(Transformers.AliasToBean<rezult>())
                .List<rezult>();

            rezult.ToList().ForEach(x =>
            {
                var specs = new Dictionary<int, string>();
                if (dictionary.ContainsKey(x.ObjectId))
                {
                    specs = dictionary[x.ObjectId];
                }
                else
                {
                    dictionary.Add(x.ObjectId, specs);
                }

                specs.Add(x.SpecificationId, x.Value);

            });

            return dictionary;

        }

        public User FindUserByLogin(string login)
        {
            return Session.Query<User>()
                .Where(x => x.Login == login).FirstOrDefault();
        }

        private string operationalReportQuery = @"
                select
	                 r.MaintenanceObjectId as Id,
	                mo.TechIndex,
	                p.DepartmentId,
	                mo.PlantId,
	                r.StartMaintenance,
	                r.EndMaintenance,
	                r.UsageBeforeMaintenance,
	                r.UsageAfterMaintenance,
	                r.[State],
	                mp.MaintenanceTypeId as PlannedMaintenanceType,
	                r.ActualMaintenanceType,
	                r.UnplannedReason,
	                r.OfferForPlan,
	                r.ReasonForOffer,
	                r.period as Period
                from OperationalReport r
                LEFT JOIN MaintenanceObject mo ON mo.MaintenanceObjectId = r.MaintenanceObjectId
                LEFT JOIN Plant p ON p.PlantId = mo.PlantId
                LEFT JOIN MaintenancePlan mp ON mp.MaintenanceObjectId = r.MaintenanceObjectId AND YEAR(mp.MaintenanceDate)*100+MONTH(mp.MaintenanceDate) = r.period
        ";

        public IEnumerable<OperationalReportDto> FindOperationalReportAll()
        {
            return Session.CreateSQLQuery(operationalReportQuery)
               .SetResultTransformer(Transformers.AliasToBean<OperationalReportDto>())
               .List<OperationalReportDto>();

        }

        public OperationalReportDto FindOperationalReportByObjectId(int objectId)
        {
            return Session.CreateSQLQuery(operationalReportQuery + " WHERE mo.MaintenanceObjectId = :id")
                .SetParameter("id", objectId)
                .SetResultTransformer(Transformers.AliasToBean<OperationalReportDto>())
                .List<OperationalReportDto>().FirstOrDefault();
        }

        public IEnumerable<OperationalReportDto> FindOperationalReportByParams(Period period, IEnumerable<ObjectGroup> groups, IEnumerable<Plant> plants)
        {
                return Session.CreateSQLQuery(operationalReportQuery+ " WHERE r.period = :period AND mo.PlantId in (:plants) AND mo.ObjectGroupId in (:groups)")
                .SetParameter("period",period.period)
                .SetParameterList("plants",plants.Select(x=>x.Id))
                .SetParameterList("groups",groups.Select(x=>x.Id))
                .SetResultTransformer(Transformers.AliasToBean<OperationalReportDto>())
                .List<OperationalReportDto>();

        }

        private string planQuery = @"
            SELECT 
	            p.MaintenancePlanId AS Id,
	            mo.MaintenanceObjectId AS ObjectId,
	            mo.ObjectGroupId AS GroupId,
	            mo.TechIndex,
	            pl.DepartmentId,
	            mo.PlantId,
	            p.MaintenanceDate,
	            p.MaintenanceTypeId,
	            p.MaintenanceReasonId
            FROM 
            MaintenancePlan p
            LEFT JOIN MaintenanceObject AS mo ON p.MaintenanceObjectId = mo.MaintenanceObjectId
            LEFT JOIN Plant AS pl ON pl.PlantId = mo.PlantId
        ";

        public IEnumerable<MaintenancePlanDto> FindMaintenancePlanAll()
        {
            return Session.CreateSQLQuery(planQuery)
               .SetResultTransformer(Transformers.AliasToBean<MaintenancePlanDto>())
               .List<MaintenancePlanDto>();
        }

        public IEnumerable<MaintenancePlanDto> FindMaintenancePlanByPeriod(Period period)
        {
            return Session.CreateSQLQuery(planQuery + " WHERE p.MaintenanceDate between :start and :end")
               .SetParameter("start", period.Start())
               .SetParameter("end", period.End())
               .SetResultTransformer(Transformers.AliasToBean<MaintenancePlanDto>())
               .List<MaintenancePlanDto>();
        }

        public IEnumerable<GroupIntervalDto> FindGroupInterval()
        {
            return Session.CreateSQLQuery(@"
                SELECT
	                p.ObjectGroupId AS ParentId,
	                p.Name AS ParentName,
	                g.ObjectGroupId AS Id,
	                g.Name AS GroupName,
	                o.MinUsage AS O_min,
	                o.MaxUsage AS O_max,
	                o.QuantityInCycle AS O_qt,
	                t.MinUsage AS T_min,
	                t.MaxUsage AS T_max,
	                t.QuantityInCycle AS T_qt,
	                s.MinUsage AS S_min,
	                s.MaxUsage AS S_max,
	                s.QuantityInCycle AS S_qt,
	                k.MinUsage AS K_min,
	                k.MaxUsage AS K_max,
	                k.QuantityInCycle AS K_qt
                FROM ObjectGroup g
                LEFT JOIN MaintenanceInterval AS o ON o.ObjectGroupId = g.ObjectGroupId AND o.MaintenanceTypeId = 1
                LEFT JOIN MaintenanceInterval AS t ON t.ObjectGroupId = g.ObjectGroupId AND t.MaintenanceTypeId = 2
                LEFT JOIN MaintenanceInterval AS s ON s.ObjectGroupId = g.ObjectGroupId AND s.MaintenanceTypeId = 3
                LEFT JOIN MaintenanceInterval AS k ON k.ObjectGroupId = g.ObjectGroupId AND k.MaintenanceTypeId = 4
                LEFT JOIN ObjectGroup p ON g.PARENT_ID = p.ObjectGroupId
                ORDER BY p.ObjectGroupId, g.ObjectGroupId
                ")
               .SetResultTransformer(Transformers.AliasToBean<GroupIntervalDto>())
               .List<GroupIntervalDto>();
        }

        public IEnumerable<UsageDto> FindUsage()
        {
            return Session.CreateSQLQuery(@"
                 SELECT
                    mo.MaintenanceObjectId AS Id,
	                mo.ObjectGroupId AS GroupId,
	                mo.PlantId,
	                p.DepartmentId,
	                mo.TechIndex,
	                mo.CurrentOperatingState,
	                o.LastMaintenanceDate AS o_date,
	                o.UsageFromLastMaintenance AS o_usage,
	                t.LastMaintenanceDate AS t_date,
	                t.UsageFromLastMaintenance AS t_usage,
	                s.LastMaintenanceDate AS s_date,
	                s.UsageFromLastMaintenance AS s_usage,
	                k.LastMaintenanceDate AS k_date,
	                k.UsageFromLastMaintenance AS k_usage
                FROM MaintenanceObject AS mo
                LEFT JOIN Plant AS p ON p.PlantId = mo.PlantId
                LEFT JOIN LastMaintenance AS o ON o.MaintenanceObjectId = mo.MaintenanceObjectId AND o.MaintenanceTypeId = 1
                LEFT JOIN LastMaintenance AS t ON t.MaintenanceObjectId = mo.MaintenanceObjectId AND t.MaintenanceTypeId = 2
                LEFT JOIN LastMaintenance AS s ON s.MaintenanceObjectId = mo.MaintenanceObjectId AND s.MaintenanceTypeId = 3
                LEFT JOIN LastMaintenance AS k ON k.MaintenanceObjectId = mo.MaintenanceObjectId AND k.MaintenanceTypeId = 4
                ")
          .SetResultTransformer(Transformers.AliasToBean<UsageDto>())
          .List<UsageDto>();

        }
    }
}
