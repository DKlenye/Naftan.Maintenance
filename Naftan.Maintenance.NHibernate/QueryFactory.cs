using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.NHibernate;
using NHibernate;
using NHibernate.Linq;
using Naftan.Maintenance.Domain.Specifications;

namespace Naftan.Maintenance.NHibernate
{
    public class QueryFactory:IQueryFactory
    {
        private readonly ISessionProvider provider;
       
        public QueryFactory(ISessionProvider provider)
        {
            this.provider = provider;
        }

        private ISession session => provider.CurrentSession;
  
        
        public IEnumerable<ObjectGroup> FindObjectGroups()
        {
            return session.Query<ObjectGroup>()
                .Where(x => x.Parent == null)
                .ToArray();
        }

        public IEnumerable<MaintenanceObject> FindObjects()
        {
            return session.QueryOver<MaintenanceObject>()
                .Fetch(x => x.Plant).Eager
                .Fetch(x=>x.Plant.Department).Eager
                .Fetch(x=>x.Manufacturer).Eager
                .Fetch(x=>x.Environment).Eager
                .Fetch(x=>x.Group).Eager
                .List();
        }

        public IEnumerable<ObjectSpecification> FindObjectSpecifications(int[] specificationId)
        {
            return session.QueryOver<ObjectSpecification>()
                .WhereRestrictionOn(x => x.Specification.Id)
                    .IsIn(specificationId)
                .List();
        }
    }
}
