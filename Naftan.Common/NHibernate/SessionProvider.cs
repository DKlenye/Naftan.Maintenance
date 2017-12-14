using NHibernate;

namespace Naftan.Common.NHibernate
{
    public class SessionProvider:ISessionProvider
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;
        ///<summary>
        ///  ctor
        ///</summary>
        ///<param name="sessionFactory"> </param>
        public SessionProvider(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }


        public ISession CurrentSession
        {
            get
            {
                if (_session==null )
                    _session = _sessionFactory.OpenSession();
                return _session;
            }
        }
    }
}
