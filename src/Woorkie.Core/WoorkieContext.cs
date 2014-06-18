using System;
using Castle.Windsor;
using Woorkie.Core.Nhibernate;

namespace Woorkie.Core
{
    public class WoorkieContext : IWoorkieContext, IDisposable
    {
        private readonly WindsorContainer _container;
        private IAnalyzer _analyzer;
        private IDbContext _dbContext;

        public WoorkieContext(IConnectionStringProvider connectionStringProvider)
        {
            if (connectionStringProvider == null)
                throw new ArgumentNullException("connectionStringProvider");

            _container = new WindsorContainer();
            _container.Install(new DefaultWoorkieWindsorInstaller(connectionStringProvider));

            _dbContext = _container.Resolve<IDbContext>();
            _analyzer = _container.Resolve<IAnalyzer>();
        }

        public IWorkEntry AddWork(IProfile profile, string label, DateTime start, TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public IProfile CreateProfile(string name)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public IProfile FindProfile(string name)
        {
            throw new NotImplementedException();
        }

        public IAnalyzer GetAnalyzer(string profile)
        {
            throw new NotImplementedException();
        }
    }
}
