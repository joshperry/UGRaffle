using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RaffleLib.Domain.Entities;
using FluentNHibernate.Automapping.Alterations;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories.MappingOverrides
{
    public class MemberOverride : IAutoMappingOverride<Member>
    {
        public void Override(FluentNHibernate.Automapping.AutoMapping<Member> mapping)
        {
            mapping.Map(x => x.Roles).CustomType<CsvUserType>();
        }
    }
}
