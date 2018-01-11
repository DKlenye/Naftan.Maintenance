using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto.Objects;
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

        public IEnumerable<OperationalReportDto> Get()
        {
            return query.FindOperationalReport();
        }

        [HttpGet, Route("api/object/addReports")]
        public void AddReports()
        {
            var objects = query.FindObjects();

            objects.ToList().ForEach(x =>
            {
                var o = repository.Get<MaintenanceObject>(x.Id);

                o.Report = new OperationalReport
                {
                    MaintenanceObject = o,
                    Period = Period.Now(),
                    UsageBeforeMaintenance = Period.Now().Hours(),
                    State = o.CurrentOperatingState??OperatingState.Operating
                };

                repository.Save(o);

            });
        }


        [HttpGet, Route("api/object/applyReports")]
        public void ApplyReports()
        {
            var objects = query.FindObjects().Take(200);

            objects.ToList().ForEach(x =>
            {
                var o = repository.Get<MaintenanceObject>(x.Id);

                o.ApplyReport();

                repository.Save(o);

            });
        }


    }
}
