using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;

namespace RaffleWeb.Infrastructure
{
    public class PersistenceConfigurerModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<FluentNHibernate.Cfg.Db.IPersistenceConfigurer>()
                .ToMethod(c =>
                    FluentNHibernate.Cfg.Db.SQLiteConfiguration
                        .Standard
                        .ConnectionString(cn => cn.FromConnectionStringWithKey("ormConnection"))
                        );
        }
    }
}