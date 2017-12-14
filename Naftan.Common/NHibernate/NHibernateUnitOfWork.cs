using System;
using System.Data;
using Naftan.Common.Domain;
using NHibernate;

namespace Naftan.Common.NHibernate
{
    
        public class NHibernateUnitOfWork : IUnitOfWork, IDisposable
        {
            private readonly ISession _session;
            private ITransaction _transaction;

            public NHibernateUnitOfWork(ISession session, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
            {
                if (session == null)
                    throw new ArgumentNullException("session");

                _session = session;
                _transaction = session.BeginTransaction(isolationLevel);
            }


            public void Dispose()
            {
                if (!_transaction.WasCommitted && !_transaction.WasRolledBack)
                    _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;

                //_session.Dispose();
            }

            public void Commit()
            {
                _transaction.Commit();
            }
         

        
    }
}
