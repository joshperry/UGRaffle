using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace RaffleWeb.Infrastructure
{
    public class QueryModelBinder : IModelBinder
    {
        private Type _concreteQueryType;
        public QueryModelBinder(Type concreteQueryType)
        {
            _concreteQueryType = concreteQueryType;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new InvalidOperationException();

            return MvcApplication.Kernel.TryGet(_concreteQueryType);
        }

        internal static void AddAllBinders(System.Reflection.Assembly assembly)
        {
            var queryTypes = assembly.GetExportedTypes()
                .Where(t => typeof(RaffleLib.Domain.Queries.IQuery).IsAssignableFrom(t));

            foreach(var queryType in queryTypes)
            {
                var queryInterface = queryType.GetInterfaces().Where(i => i.Name.EndsWith(queryType.Name)).FirstOrDefault();
                if(queryInterface != null)
                    ModelBinders.Binders.Add(queryInterface, new RaffleWeb.Infrastructure.QueryModelBinder(queryType));
            }
        }
    }
}