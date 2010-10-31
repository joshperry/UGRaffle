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
                new WebLib.Ninject.NinjectControllerModule(new WebLib.Ninject.ConventionFindControllersStrategy(System.Reflection.Assembly.GetExecutingAssembly())),
                new Infrastructure.PersistenceConfigurerModule(),
                new RaffleLib.Domain.Repositories.NHibernateRepositories.NHibernateConfigModule(),
                new RaffleLib.Domain.Repositories.NHibernateRepositories.NHibernateRepositoryModule(),
                new WebLib.SessionPerRequestModule(),
                new Infrastructure.AuthModule()
            );

            ControllerBuilder.Current.SetControllerFactory(new WebLib.Ninject.NinjectControllerFactory(Kernel));
            AddQueryModelBinders(typeof(RaffleLib.Domain.Queries.IQuery).Assembly, Kernel);
        }

        public static void AddQueryModelBinders(System.Reflection.Assembly assembly, IKernel kernel)
        {
            var queryTypes = assembly.GetExportedTypes()
                .Where(t => typeof(RaffleLib.Domain.Queries.IQuery).IsAssignableFrom(t));

            foreach (var queryType in queryTypes)
            {
                var queryInterface = queryType.GetInterfaces().Where(i => i.Name.EndsWith(queryType.Name)).FirstOrDefault();
                if (queryInterface != null)
                    ModelBinders.Binders.Add(queryInterface, new WebLib.Ninject.NinjectModelBinder(queryType, kernel));
            }
        }
    }
}