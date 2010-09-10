using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ninject;
using NHibernate.Cfg;
using NHibernate;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateConfigModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<NHibernateConfigurator>().ToSelf();

            Kernel.Bind<Configuration>()
                .ToConstant(Kernel.Get<NHibernateConfigurator>().Configuration);

            Kernel.Bind<ISessionFactory>()
                .ToConstant(Kernel.Get<NHibernateConfigurator>().Factory);
        }
    }
}
