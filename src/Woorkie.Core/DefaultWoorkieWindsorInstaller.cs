using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Woorkie.Core
{
    public class DefaultWoorkieWindsorInstaller : IWindsorInstaller
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public DefaultWoorkieWindsorInstaller(IConnectionStringProvider connectionStringProvider)
        {
            if (connectionStringProvider == null)
                throw new ArgumentNullException("connectionStringProvider");

            _connectionStringProvider = connectionStringProvider;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConnectionStringProvider>()
                                        .Instance(_connectionStringProvider));
        }
    }
}
