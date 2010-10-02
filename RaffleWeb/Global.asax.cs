using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace RaffleWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IKernel Kernel { get; private set; }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Registration", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

            Kernel = new StandardKernel(
                new WebLib.Ninject.NinjectControllerModule(System.Reflection.Assembly.GetExecutingAssembly()),
                new WebLib.PersistenceConfigurerModule(),
                new RaffleLib.Domain.Repositories.NHibernateRepositories.NHibernateConfigModule(),
                new RaffleLib.Domain.Repositories.NHibernateRepositories.NHibernateRepositoryModule(),
                new WebLib.SessionPerRequestModule(),
                new WebLib.Auth.AuthModule()
            );

            ControllerBuilder.Current.SetControllerFactory(new WebLib.Ninject.NinjectControllerFactory(Kernel));
            WebLib.QueryModelBinder.AddAllBinders(typeof(RaffleLib.Domain.Queries.IQuery).Assembly, Kernel);
        }
    }
}