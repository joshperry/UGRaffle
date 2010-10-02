using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;
using NHibernate;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateRaffleEntryRepository : NHibernateRepositoryBase<RaffleEntry>
    {
        public NHibernateRaffleEntryRepository(ISession session)
            : base(session)
        { }
    }
}
