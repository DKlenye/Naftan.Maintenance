using Naftan.Common.Domain;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Dto;
using Naftan.Maintenance.Domain.Dto.Objects;
using System.Collections.Generic;
using System.Web.Http;

namespace Naftan.Maintenance.WebApplication.Controllers.DtoControllers
{
    public class UsageController:ApiController
    {
        private IQueryFactory query;
        private IRepository repository;

        public UsageController(IQueryFactory query, IRepository repository)
        {
            this.query = query;
            this.repository = repository;
        }

        public IEnumerable<UsageDto> Get()
        {
            return query.FindUsage();
        }
    }
}