﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;
using NHibernate;
using NHibernate.Linq;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateMemberRepository : NHibernateRepositoryBase<Member>
    {
        public NHibernateMemberRepository(ISession session)
            : base(session)
        { }
    }
}
