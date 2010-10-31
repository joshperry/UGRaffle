using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using System.Text;

namespace WebLib.Ninject
{
    public class NinjectControllerFactory : IControllerFactory
    {
        IControllerFactory _defaultFactory;
        IKernel _kernel;
        public NinjectControllerFactory(IKernel kernel)
            :this(kernel, new DefaultControllerFactory())
        {
        }

        public NinjectControllerFactory(IKernel kernel, IControllerFactory defaultFactory)
        {
            _kernel = kernel;
            _defaultFactory = defaultFactory;
        }

        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            IController controller = null;

            // Get controllers in the container who's names start with controllerName
            var controllers = from c in _kernel.GetBindings(typeof(IController))
                              where c.Service.Name.StartsWith(controllerName)
                              select c.Service;

            switch (controllers.Count())
            {
                case 0: // No controllers in the container, let the default factory have a whack at it
                    controller = _defaultFactory.CreateController(requestContext, controllerName);
                    break;

                case 1: // Only one found, get an instance
                    controller = _kernel.Get(controllers.First()) as IController;
                    break;

                default: // Found more than one viable controller (perhaps in different namespaces), punt
                    var ambiguousControllers = controllers.Aggregate(new StringBuilder(), (sb, c) => sb.AppendFormat("{0}{1}", Environment.NewLine, c.FullName), (sb) => sb.ToString());
                    throw new InvalidOperationException(string.Format("Ambiguous controller types found: {0}", ambiguousControllers));
            }

            return controller;
        }

        public void ReleaseController(IController controller)
        {
            // If the container was tracking this instance release it
            // otherwise pass it down to the default factory to release
            if (!_kernel.Release(controller))
                _defaultFactory.ReleaseController(controller);
        }
    }
}