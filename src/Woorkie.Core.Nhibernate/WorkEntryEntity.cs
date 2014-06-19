using System;

namespace Woorkie.Core.Nhibernate
{
    public class WorkEntryEntity
    {
        public virtual TimeSpan Duration { get; set; }
        public virtual Guid Id { get; set; }
        public virtual string Label { get; set; }
        public virtual ProfileEntity Profile { get; set; }
        public virtual DateTime Start { get; set; }
    }
}
