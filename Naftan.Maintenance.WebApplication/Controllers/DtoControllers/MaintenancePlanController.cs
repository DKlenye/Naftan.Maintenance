using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto;
using Naftan.Maintenance.Domain.Dto.Objects;
using System.Collections.Generic;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class MaintenancePlanController:ApiController
    {
        private IQueryFactory query;
        private IRepository repository;

        public MaintenancePlanController(IQueryFactory query, IRepository repository)
        {
            this.query = query;
            this.repository = repository;
        }

        
        public IEnumerable<MaintenancePlanDto> Get(int id)
        {
            return query.FindMaintenancePlanByPeriod(new Period(id));
        }

    }
}