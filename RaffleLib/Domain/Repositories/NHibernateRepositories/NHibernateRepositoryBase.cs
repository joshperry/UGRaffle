using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateRepositoryBase<T> : IEntityRepository<T>
    {
        private ISession _session;
        public NHibernateRepositoryBase(ISession session)
        {
            _session = session;
        }

        public IQueryable<T> Query
        {
            get { return _session.Query<T>(); }
        }

        public void Save(T entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public void Delete(T entity)
        {
            _session.Delete(entity);
        }
    }
}
