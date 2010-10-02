using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace WebLib
{
    public class QueryModelBinder : IModelBinder
    {
        private Type _concreteQueryType;
        private IKernel _kernel;
        public QueryModelBinder(Type concreteQueryType, IKernel kernel)
        {
            _concreteQueryType = concreteQueryType;
            _kernel = kernel;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new InvalidOperationException();

            return _kernel.TryGet(_concreteQueryType);
        }

        public static void AddAllBinders(System.Reflection.Assembly assembly, IKernel kernel)
        {
            var queryTypes = assembly.GetExportedTypes()
                .Where(t => typeof(RaffleLib.Domain.Queries.IQuery).IsAssignableFrom(t));

            foreach(var queryType in queryTypes)
            {
                var queryInterface = queryType.GetInterfaces().Where(i => i.Name.EndsWith(queryType.Name)).FirstOrDefault();
                if(queryInterface != null)
                    ModelBinders.Binders.Add(queryInterface, new QueryModelBinder(queryType, kernel));
            }
        }
    }
}