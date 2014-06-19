using System;

namespace Woorkie.Core
{
    public abstract class WoorkieContextBase : IWoorkieContext, IDisposable
    {
        public IWorkEntry AddWork(IProfile profile, string label, DateTime start, TimeSpan duration)
        {
            return GetDbContext().AddWork(profile, label, start, duration);
        }

        public IProfile CreateProfile(string name)
        {
            return GetDbContext().CreateProfile(name);
        }

        public abstract void Dispose();

        public IProfile FindProfile(string name)
        {
            return GetDbContext().FindProfile(name);
        }

        public IAnalyzer GetAnalyzer(string profile)
        {
            return GetAnalyzer();
        }

        protected abstract IAnalyzer GetAnalyzer();
        protected abstract IDbContext GetDbContext();
    }
}
