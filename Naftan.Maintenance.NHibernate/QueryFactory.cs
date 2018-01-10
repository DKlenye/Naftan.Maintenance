using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Common.NHibernate;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using Naftan.Maintenance.Domain.Users;

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
                .Fetch(x => x.Plant.Department).Eager
                .Fetch(x => x.Group).Eager
                .Fetch(x => x.Report).Eager
                .List();
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

            var rezult =  session.CreateSQLQuery
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
            return session.Query<User>()
                .Where(x => x.Login == login).FirstOrDefault();
        }
    }
}
