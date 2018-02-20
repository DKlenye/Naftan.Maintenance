using Naftan.Common.Domain;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
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
            var context = UserPrincipal.Current;
            return query.FindMaintenancePlanByParams(new Period(id),context.SamAccountName);
        }

        public void Delete(int id)
        {
            repository.Remove<MaintenancePlan>(id);
        }

    }
}