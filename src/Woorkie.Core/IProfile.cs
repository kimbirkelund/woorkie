using System;

namespace Woorkie.Core
{
    public interface IProfile : IEquatable<IProfile>
    {
        TimeSpan? DefaultHoursPerWeek { get; }
        string Name { get; }

        IProfile Save();

        IProfile WithDefaultHoursPerWeek(TimeSpan? value);
    }
}
