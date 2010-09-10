using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Ninject;
using NHibernate;

namespace RaffleWeb.Infrastructure
{
    public class SessionPerRequestModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISession>()
                .ToMethod(c => {
                        var session = c.Kernel.Get<ISessionFactory>().OpenSession();
                        session.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                        return session;
                    })
                 .InRequestScope()
                 .OnDeactivation(session =>
                 {
                     try
                     {
                         if (session.Transaction != null)
                             session.Transaction.Commit();
                     }
                     catch (Exception)
                     {
                         session.Transaction.Rollback();
                     }
                     finally
                     {
                         session.Close();
                         session.Dispose();
                     }
                 });
        }
    }
}