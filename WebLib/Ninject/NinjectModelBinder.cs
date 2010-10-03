using System;
using System.Web.Mvc;
using Ninject;

namespace WebLib.Ninject
{
    public class NinjectModelBinder : IModelBinder
    {
        private readonly Type _concreteType;
        private readonly IKernel _kernel;
        public NinjectModelBinder(Type concreteType, IKernel kernel)
        {
            _concreteType = concreteType;
            _kernel = kernel;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.Model != null)
                throw new InvalidOperationException();

            return _kernel.TryGet(_concreteType);
        }
    }
}