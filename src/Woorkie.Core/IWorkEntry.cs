using System;

namespace Woorkie.Core
{
    public interface IWorkEntry : IEquatable<IWorkEntry>
    {
        TimeSpan Duration { get; }
        Guid Id { get; }
        string Label { get; }
        IProfile Profile { get; }
        DateTime Start { get; }

        IWorkEntry Save();

        IWorkEntry WithDuration(TimeSpan value);
        IWorkEntry WithLabel(string value);
        IWorkEntry WithStart(DateTime value);
    }
}
