using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using WebLib.Ninject;
using Moq;
using Ninject;
using System.Web.Routing;
using System.Web;

namespace WebLibTests
{
    public class DummyController : IController
    {
        public void Execute(System.Web.Routing.RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class NinjectControllerFactoryTests
    {
        [TestMethod]
        public void Can_find_controller_by_convention()
        {
            var finder = new ConventionControllerFinder(System.Reflection.Assembly.GetExecutingAssembly());

            var controllers = finder.GetControllers().ToList();

            CollectionAssert.Contains(controllers, typeof(DummyController));
        }

        [TestMethod]
        public void Module_injects_controllers()
        {
            var module = new NinjectControllerModule(System.Reflection.Assembly.GetExecutingAssembly());
            using (var kernel = new StandardKernel())
            {
                module.OnLoad(kernel);

                Assert.AreNotEqual(0, kernel.GetBindings(typeof(DummyController)).Count());
            }
        }

    }
}
