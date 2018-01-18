using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto.Groups;
using System.Collections.Generic;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class GroupIntervalController:ApiController
    {
        private IQueryFactory query;
        private IRepository repository;

        public GroupIntervalController(IQueryFactory query, IRepository repository)
        {
            this.query = query;
            this.repository = repository;
        }

        public IEnumerable<GroupIntervalDto> Get()
        {
            return query.FindGroupInterval();
        }
    }
}