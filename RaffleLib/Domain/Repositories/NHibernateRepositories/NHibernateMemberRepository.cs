using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateMemberRepository : IEntityRepository<Member>
    {
        private ISession _session;
        public NHibernateMemberRepository(ISession session)
        {
            _session = session;
        }

        public IQueryable<Member> Query
        {
            get { return _session.Query<Member>(); }
        }

        public void Save(Member entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public void Delete(Member entity)
        {
            _session.Delete(entity);
        }
    }
}
