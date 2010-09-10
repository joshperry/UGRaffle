using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateMeetingRepository : IEntityRepository<Meeting>
    {
        private ISession _session;
        public NHibernateMeetingRepository(ISession session)
        {
            _session = session;
        }

        public IQueryable<Meeting> Query
        {
            get { return _session.Query<Meeting>(); }
        }

        public void Save(Meeting entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public void Delete(Meeting entity)
        {
            _session.Delete(entity);
        }
    }
}
