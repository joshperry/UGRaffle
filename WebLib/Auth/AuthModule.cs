using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using RaffleLib.Security;
using System.Web.Security;

namespace WebLib.Auth
{
    public class AuthModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IFormsAuthentication>().To<FormsAuthenticationService>().InSingletonScope();
            Kernel.Bind<IHasher>().To<Sha256Hasher>().InSingletonScope();
            Kernel.Bind<RoleProvider>().To<MemberRoleProvider>();
        }
    }
}