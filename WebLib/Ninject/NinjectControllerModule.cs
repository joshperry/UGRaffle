using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using System.Reflection;
using System.Web.Mvc;

namespace WebLib.Ninject
{
    public class NinjectControllerModule : NinjectModule
    {
        IFindControllersStrategy _finder;
        public NinjectControllerModule(IFindControllersStrategy finder)
        {
            _finder = finder;
        }

        public override void Load()
        {
            foreach (var controller in _finder.GetControllers())
            {
                Kernel.Bind(controller).To<IController>();
            }
        }
    }
}