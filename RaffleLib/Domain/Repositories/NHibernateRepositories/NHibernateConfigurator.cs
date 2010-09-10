using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using FluentNHibernate.Automapping;
using RaffleLib.Domain.Entities;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateConfigurator
    {
        public NHibernateConfigurator(IPersistenceConfigurer dbconf)
        {
            Factory = GetSessionFactory(dbconf);
        }

        private ISessionFactory GetSessionFactory(IPersistenceConfigurer dbconf)
        {
            var map = AutoMap
                .AssemblyOf<Meeting>()
                .Where(e => e.Namespace.EndsWith("Entities"))
                .Conventions.AddFromAssemblyOf<Meeting>()
                .UseOverridesFromAssemblyOf<Meeting>();

            var factory = Fluently.Configure()
                .Database(dbconf)
                .Mappings(m => m.AutoMappings.Add(map))
                .ExposeConfiguration(c => Configuration = c)
                .BuildSessionFactory();

            return factory;
        }

        public ISessionFactory Factory { get; private set; }
        public Configuration Configuration { get; private set; }
    }
}
