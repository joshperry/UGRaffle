using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLib.Ninject
{
    public interface IFindControllersStrategy
    {
        IEnumerable<Type> GetControllers();
    }
}
