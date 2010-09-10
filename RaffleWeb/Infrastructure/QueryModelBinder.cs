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
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new InvalidOperationException();

            return MvcApplication.Kernel.TryGet(bindingContext.ModelType);
        }

        internal static void AddAllBinders(System.Reflection.Assembly assembly)
        {
            var queries = assembly.GetExportedTypes()
                .Where(t => typeof(RaffleLib.Domain.Queries.IQuery).IsAssignableFrom(t));

            foreach(var query in queries)
                ModelBinders.Binders.Add(query, new RaffleWeb.Infrastructure.QueryModelBinder());
        }
    }
}