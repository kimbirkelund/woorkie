using System;

namespace Woorkie.Core.Nhibernate
{
    public class NhProfile
    {
        public virtual TimeSpan? DefaultHoursPerWeek { get; set; }
        public virtual string Name { get; set; }
    }
}
