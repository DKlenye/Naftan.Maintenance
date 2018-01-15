using Naftan.Common.AccountManagement;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto.Objects;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class OperationalReportController : ApiController
    {
        private IQueryFactory query;
        private IRepository repository;

        public OperationalReportController(IQueryFactory query, IRepository repository)
        {
            this.query = query;
            this.repository = repository;
        }

        [HttpGet, Route("api/operationalReport/{period}")]
        public IEnumerable<OperationalReportDto> Get(int period)
        {
            var account = ActiveDirectory.CurrentAccount;
            var user = query.FindUserByLogin(account.Login);


            if (user.Plants.Any() && user.ObjectGroups.Any())
            {
                return query.FindOperationalReportByParams(period, user.ObjectGroups, user.Plants);
            }

            return new List<OperationalReportDto>();

        }

        [HttpPut, Route("api/operationalReport/{id}")]
        public OperationalReportDto Put(int id, [FromBody] OperationalReportDto dto)
        {
            var entity = repository.Get<MaintenanceObject>(id);
            var report = entity.Report;

            report.StartMaintenance = dto.StartMaintenance;
            report.EndMaintenance = dto.EndMaintenance;
            report.UsageBeforeMaintenance = dto.UsageBeforeMaintenance;
            report.UsageAfterMaintenance = dto.UsageAfterMaintenance;
            report.State = dto.State;
            report.OfferForPlan = dto.OfferForPlan==null? null : repository.Get<MaintenanceType>(dto.OfferForPlan.Value);
            report.ReasonForOffer = dto.ReasonForOffer==null? null: repository.Get<MaintenanceReason>(dto.ReasonForOffer.Value);
            report.PlannedMaintenanceType = dto.PlannedMaintenanceType==null? null:repository.Get<MaintenanceType>(dto.PlannedMaintenanceType.Value);
            report.UnplannedReason = dto.UnplannedReason==null? null: repository.Get<MaintenanceReason>(dto.UnplannedReason.Value);
            report.ActualMaintenanceType = dto.ActualMaintenanceType == null ? null : repository.Get<MaintenanceType>(dto.ActualMaintenanceType.Value);

            repository.Save(entity);

            return query.FindOperationalReportByObjectId(entity.Id);
        }


        [HttpPost, Route("api/operationalReport/applyReports")]
        public IEnumerable<int> ApplyReports([FromBody] ListSerializer<int> list)
        {
            list.data.ToList().ForEach(x =>
            {
                var o = repository.Get<MaintenanceObject>(x);
                o.ApplyReport();
                repository.Save(o);
            });

            return list.data;

        }






    }
}
