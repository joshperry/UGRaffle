using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;

namespace WebLib.Ninject
{
    public class ConventionControllerFinder
    {
        IEnumerable<Type> _controllers;
        public ConventionControllerFinder(params Assembly[] assemblies)
        {
            _controllers =
                from a in assemblies
                from c in a.GetExportedTypes()
                where !c.IsAbstract
                    && !c.IsInterface
                    && c.Name.EndsWith("Controller")
                    && typeof(IController).IsAssignableFrom(c)
                select c;
        }

        public IEnumerable<Type> GetControllers()
        {
            return _controllers;
        }
    }
}