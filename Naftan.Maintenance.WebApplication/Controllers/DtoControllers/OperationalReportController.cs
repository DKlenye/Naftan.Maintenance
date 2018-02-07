using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Objects;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
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
            var context = UserPrincipal.Current;
            return query.FindOperationalReportByParams(new Period(period), context.SamAccountName).ToList(); 
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
            report.UnplannedReason = dto.UnplannedReason==null? null: repository.Get<MaintenanceReason>(dto.UnplannedReason.Value);
            report.ActualMaintenanceType = dto.ActualMaintenanceType == null ? null : repository.Get<MaintenanceType>(dto.ActualMaintenanceType.Value);

            repository.Save(entity);

            return query.FindOperationalReportByObjectId(entity.Id);
        }


        [HttpPost, Route("api/operationalReport/applyReports/{period}")]
        public IEnumerable<int> ApplyReports(int period, [FromBody] ListSerializer<int> list)
        {
            list.data.ToList().ForEach(x =>
            {
                var o = repository.Get<MaintenanceObject>(x);
                if (o.Report.Period.period == period)
                {
                    o.ApplyReport();
                    repository.Save(o);
                }
            });

            return list.data;

        }

        [HttpPost, Route("api/operationalReport/discardReports/{period}")]
        public IEnumerable<int> DiscardReports(int period, [FromBody] ListSerializer<int> list)
        {
            
                list.data.ToList().ForEach(x =>
                {
                    var o = repository.Get<MaintenanceObject>(x);
                    if (o.Report.Period.period == period)
                    {
                        o.DiscardReport();
                        repository.Save(o);
                    }
                });

                return list.data;
            
        }

        [HttpGet, Route("api/operationalReport/setNextMaintenance")]
        public void SetNextMaintenance()
        {
            repository.All<MaintenanceObject>()
                .Where(x=>x.Id>= 4735)
                .ToList().ForEach(x => x.SetNextMaintenance());
        }

        [HttpGet, Route("api/operationalReport/plan")]
        public void Plan()
        {
            var period = new Period(201802);

            repository.All<MaintenanceObject>()
                .ToList().ForEach(x =>
                {
                    var plan = x.Plans.FirstOrDefault(p => p.MaintenanceDate >= period.Start() && p.MaintenanceDate <= period.End());

                    if (plan == null)
                    {
                        x.PlanningMaintenance(period);
                        repository.Save(x);
                    }

                });
        }

    }
}
