using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace RaffleWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static IKernel Kernel { get; private set; }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Meeting", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            Kernel = new StandardKernel(
                new RaffleWeb.Infrastructure.NinjectControllerFactory.NinjectControllerModule(System.Reflection.Assembly.GetExecutingAssembly()),
                new RaffleWeb.Infrastructure.PersistenceConfigurerModule(),
                new RaffleLib.Domain.Repositories.NHibernateRepositories.NHibernateConfigModule(),
                new RaffleLib.Domain.Repositories.NHibernateRepositories.NHibernateRepositoryModule(),
                new RaffleWeb.Infrastructure.SessionPerRequestModule(),
                new RaffleWeb.Infrastructure.Auth.AuthModule()
            );

            ControllerBuilder.Current.SetControllerFactory(new RaffleWeb.Infrastructure.NinjectControllerFactory.NinjectControllerFactory(Kernel));
            RaffleWeb.Infrastructure.QueryModelBinder.AddAllBinders(typeof(RaffleLib.Domain.Queries.IQuery).Assembly);
        }
    }
}