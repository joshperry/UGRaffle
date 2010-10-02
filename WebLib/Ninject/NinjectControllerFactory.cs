using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;

namespace WebLib.Ninject
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        IKernel _kernel;
        public NinjectControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            object controller = null;

            if(controllerType == null || (controller = _kernel.Get(controllerType)) == null)
                controller = base.GetControllerInstance(requestContext, controllerType);

            return controller as IController;
        }
    }
}