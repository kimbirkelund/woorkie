using System;
using System.Linq;

namespace Woorkie.Core.Nhibernate
{
    public interface IDbContext : IDisposable
    {
        IWorkEntry AddWork(IProfile profile, string label, DateTime start, TimeSpan duration);
        IProfile CreateProfile(string name);
        IProfile FindProfile(string name);
        IQueryable<IWorkEntry> QueryWork();
    }
}
