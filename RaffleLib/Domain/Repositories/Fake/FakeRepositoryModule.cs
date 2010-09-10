using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using RaffleLib.Domain.Entities;

namespace RaffleLib.Domain.Repositories.Fake
{
    public class FakeRepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IEntityRepository<Meeting>>().To<FakeMeetingRepository>();
        }
    }
}
