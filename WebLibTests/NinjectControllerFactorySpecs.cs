using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
    [Subject(typeof(ConventionFindControllersStrategy), "Finding Controllers")]
	public class when_searching_for_controllers_that_exist {
		protected static IEnumerable<Type> controllers;

		Because of = () => controllers = new ConventionFindControllersStrategy(System.Reflection.Assembly.GetExecutingAssembly()).GetControllers();

        It should_find_only_public_instantiatable_controller = () => controllers.ShouldContainOnly(typeof(DummyController), typeof(OtherDummyController));
	}

    [Subject(typeof(ConventionFindControllersStrategy), "Finding Controllers")]
	public class when_searching_for_controllers_and_none_exist {
		protected static IEnumerable<Type> controllers;

		Because of = () => controllers = new ConventionFindControllersStrategy(System.Reflection.Assembly.GetAssembly(typeof(Machine.Specifications.It))).GetControllers();

        It should_find_no_controllers = () => controllers.ShouldBeEmpty();
	}

    [Subject(typeof(NinjectControllerModule), "Injecting Controllers")]
	public class when_loading_controller_module {
		protected static Mock<IKernel> kernelMock = new Mock<IKernel>();
        protected static Mock<Ninject.Syntax.IBindingToSyntax<object>> bindMock = new Mock<Ninject.Syntax.IBindingToSyntax<object>>();
        protected static Mock<IFindControllersStrategy> finderMock = new Mock<IFindControllersStrategy>();

        Establish context = ()=> {
            bindMock.Setup(x => x.To<IController>()).Verifiable();
            kernelMock.Setup(x => x.Bind(typeof(DummyController))).Returns(bindMock.Object);
            kernelMock.Setup(x => x.Bind(typeof(OtherDummyController))).Returns(bindMock.Object);
            finderMock.Setup(x => x.GetControllers()).Returns(new[] { typeof(DummyController), typeof(OtherDummyController) }).Verifiable();
        };

		Because of = () => {
            var module = new NinjectControllerModule(finderMock.Object);
            module.OnLoad(kernelMock.Object);
        };
        
        It should_get_controllers_from_finder = () => finderMock.Verify();

        It should_bind_controllers_to_kernel = () => kernelMock.Verify(x => x.Bind(Moq.It.IsAny<Type>()), Times.Exactly(2));

        It should_bind_controller_type_to_IController = () => bindMock.Verify();
	}

    [Subject(typeof(NinjectControllerFactory), "Building Controllers")]
	public class when_requesting_existing_controller : with_controllers {
		protected static IController result;

		Because of = () => result = new NinjectControllerFactory(kernel, defaultFactory).CreateController(null, "Dummy");

        It should_build_controller_instance = () => result.ShouldNotBeNull();
        
        It should_build_instance_of_proper_type = () => result.ShouldBeOfType<DummyController>();
	}

    [Subject(typeof(NinjectControllerFactory), "Building Controllers")]
	public class when_requesting_conflicted_controller : with_conflicting_controllers {
		protected static Exception result;

        Because of = () => result = Catch.Exception(()=> new NinjectControllerFactory(kernel, defaultFactory).CreateController(null, "Dummy"));

        It should_throw_invalid_operation_exception = () => result.ShouldBeOfType<InvalidOperationException>();
	}

    [Subject(typeof(NinjectControllerFactory), "Building Controllers")]
    public class when_requesting_noninjected_controller : with_no_controllers
    {
        protected static IController result;

        Establish context = () => defaultFactoryMock
            .Setup(x => x.CreateController(Moq.It.IsAny<RequestContext>(), "Dummy"))
            .Returns(new DummyController())
            .Verifiable();

        Because of = () => result = new NinjectControllerFactory(kernel, defaultFactory).CreateController(null, "Dummy");

        It should_query_default_factory = () => defaultFactoryMock.Verify();

        It should_return_controller_from_default_factory = () => result.ShouldNotBeNull();
    }

    [Subject(typeof(NinjectControllerFactory), "Building Controllers")]
    public class when_requesting_unavailable_controller : with_no_controllers
    {
        protected static IController result;

        Because of = () => result = new NinjectControllerFactory(kernel, defaultFactory).CreateController(null, "Dummy");

        It should_provide_null_controller = () => result.ShouldBeNull();
    }

    public class with_conflicting_controllers : with_no_controllers
    {
        Establish context = ()=>
        {
            bindings.Add(GetControllerBinding<DummyController>());
            bindings.Add(GetControllerBinding<Dummy>());
        };
    }

    public class with_controllers : with_no_controllers
    {
        Establish context = ()=>
        {
            bindings.Add(GetControllerBinding<DummyController>());
            bindings.Add(GetControllerBinding<PrivateDummyController>());
        };
    }

    public class with_no_controllers
    {
        protected static IKernel kernel;
        protected static Mock<IControllerFactory> defaultFactoryMock;
        protected static IControllerFactory defaultFactory;
        protected static List<Ninject.Planning.Bindings.IBinding> bindings;

        Establish context = () =>
        {
            bindings = new List<Ninject.Planning.Bindings.IBinding>();

            var reqMock = new Mock<Ninject.Activation.IRequest>();

            var kernelMock = new Mock<IKernel>();
            kernelMock.Setup(x => x.GetBindings(typeof(IController))).Returns(bindings);
            // Setup to mock request for an instance of DummyController
            kernelMock.Setup(x => x.CreateRequest(typeof(DummyController),
                Moq.It.IsAny<Func<Ninject.Planning.Bindings.IBindingMetadata, bool>>(),
                Moq.It.IsAny<IEnumerable<Ninject.Parameters.IParameter>>(),
                Moq.It.IsAny<bool>(), Moq.It.IsAny<bool>())).Returns(reqMock.Object);
            kernelMock.Setup(x => x.Resolve(reqMock.Object)).Returns(new[]{new DummyController()});

            kernel = kernelMock.Object;

            defaultFactoryMock = new Mock<IControllerFactory>();
            defaultFactory = defaultFactoryMock.Object;
        };

        protected static Ninject.Planning.Bindings.IBinding GetControllerBinding<T>()
        {
            var mock = new Mock<Ninject.Planning.Bindings.IBinding>();
            mock.SetupGet(x => x.Service).Returns(typeof(T));

            return mock.Object;
        }
    }

    public interface IDummyController : IController
    {
    }
    public abstract class ADummyController : IController
    {
        public abstract void Execute(RequestContext requestContext);
    }
    class PrivateDummyController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
    public class Dummy : IController
    {
        public void Execute(RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
    public class DummyController : IController
    {
        public void Execute(System.Web.Routing.RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
    public class OtherDummyController : IController
    {
        public void Execute(System.Web.Routing.RequestContext requestContext)
        {
            throw new NotImplementedException();
        }
    }

}
