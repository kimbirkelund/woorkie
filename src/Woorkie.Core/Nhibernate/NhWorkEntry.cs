using System;

namespace Woorkie.Core.Nhibernate
{
    public class NhWorkEntry
    {
        public virtual TimeSpan Duration { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string Label { get; set; }
        public virtual NhProfile Profile { get; set; }
        public virtual DateTime Start { get; set; }
    }
}
