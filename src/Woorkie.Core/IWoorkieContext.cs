using System;

namespace Woorkie.Core
{
    public interface IWoorkieContext
    {
        IWorkEntry AddWork(IProfile profile, string label, DateTime start, TimeSpan duration);
        IProfile CreateProfile(string name);
        IProfile FindProfile(string name);
        IAnalyzer GetAnalyzer(string profile);
    }
}
