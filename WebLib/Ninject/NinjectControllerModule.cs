using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using System.Reflection;

namespace WebLib.Ninject
{
    public class NinjectControllerModule : NinjectModule
    {
        ConventionControllerFinder _finder;
        public NinjectControllerModule(params Assembly[] assemblies)
        {
            _finder = new ConventionControllerFinder(assemblies);
        }

        public override void Load()
        {
            foreach (var controller in _finder.GetControllers())
            {
                Kernel.Bind(controller).ToSelf();
            }
        }
    }
}