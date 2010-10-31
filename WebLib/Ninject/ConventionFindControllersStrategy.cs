using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;

namespace WebLib.Ninject
{
    public class ConventionFindControllersStrategy : IFindControllersStrategy
    {
        IEnumerable<Type> _controllers;
        public ConventionFindControllersStrategy(params Assembly[] assemblies)
        {
            _controllers =
                from a in assemblies
                from c in a.GetExportedTypes()
                where !c.IsAbstract
                    && c.IsPublic
                    && c.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    && typeof(IController).IsAssignableFrom(c)
                select c;
        }

        public IEnumerable<Type> GetControllers()
        {
            return _controllers;
        }
    }
}