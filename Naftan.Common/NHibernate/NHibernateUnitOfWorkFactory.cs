using Naftan.Common.Domain;

namespace Naftan.Common.NHibernate
{
    public class NHibernateUnitOfWorkFactory:IUnitOfWorkFactory
    {
        private readonly ISessionProvider _provider;


        public NHibernateUnitOfWorkFactory(ISessionProvider provider)
        {
            _provider = provider;
        }

        public IUnitOfWork Create()
        {
            return new NHibernateUnitOfWork(_provider.CurrentSession);
        }
    }
}
