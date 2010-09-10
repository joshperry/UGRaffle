using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using Ninject;
using NHibernate.Cfg;
using NHibernate;
using System.Reflection;

namespace RaffleLib.Domain.Repositories.NHibernateRepositories
{
    public class NHibernateRepositoryModule : NinjectModule
    {
        public override void Load()
        {
            var repoTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => t.Name.StartsWith("NHibernate")
                    && ImplementsRepository(t));

            foreach (var repoType in repoTypes)
            {
                var interfaceType = repoType.GetInterfaces()
                    .Where(i => InterfaceIsRepository(i))
                    .First();

                Kernel.Bind(interfaceType).To(repoType);
            }
        }

        private bool InterfaceIsRepository(Type i)
        {
            return i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityRepository<>);
        }

        private bool ImplementsRepository(Type t)
        {
            return t.GetInterfaces()
                .Where(i => InterfaceIsRepository(i))
                .FirstOrDefault() != null;
        }
    }
}
