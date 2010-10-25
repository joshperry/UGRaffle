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
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace WebLibTests
{
    public class DummyController : IController
    {
        public void Execute(System.Web.Routing.RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }

    [Subject(typeof(ConventionControllerFinder), "Finding Controllers")]
	public class when_searching_for_controllers_that_exist {
		protected static IEnumerable<Type> controllers;

		Because of = () => controllers = new ConventionControllerFinder(System.Reflection.Assembly.GetExecutingAssembly()).GetControllers();

        It should_find_controller = () => controllers.ShouldContainOnly(typeof(DummyController));
	}

    [Subject(typeof(ConventionControllerFinder), "Finding Controllers")]
	public class when_searching_for_controllers_and_none_exist {
		protected static IEnumerable<Type> controllers;

		Because of = () => controllers = new ConventionControllerFinder(System.Reflection.Assembly.GetAssembly(typeof(Machine.Specifications.It))).GetControllers();

        It should_find_no_controllers = () => controllers.ShouldBeEmpty();
	}

    [Subject(typeof(NinjectControllerModule), "Injecting Controllers")]
	public class when_loading_controller_module {
		protected static Mock<IKernel> kernelMock = new Mock<IKernel>();
        protected static Mock<Ninject.Syntax.IBindingToSyntax<object>> bindMock = new Mock<Ninject.Syntax.IBindingToSyntax<object>>();

        Establish context = ()=> {
            bindMock.Setup(x => x.ToSelf()).Verifiable();
            kernelMock.Setup(x => x.Bind(typeof(DummyController))).Returns(bindMock.Object).Verifiable();
        };

		Because of = () => {
            var module = new NinjectControllerModule(System.Reflection.Assembly.GetExecutingAssembly());
            module.OnLoad(kernelMock.Object);
        };

        It should_bind_controllers_to_kernel = () => kernelMock.Verify();

        It should_bind_controller_type_to_self = () => bindMock.Verify();
	}
}
