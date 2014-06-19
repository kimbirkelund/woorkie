using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class NhWorkEntry : WorkEntryBase, IEquatable<NhWorkEntry>
    {
        private readonly ISession _session;

        public NhWorkEntry(ISession session, Guid id, IProfile profile, string label, DateTime start, TimeSpan duration)
            : base(id, profile, label, start, duration)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _session = session;
        }

        public bool Equals(NhWorkEntry other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return base.Equals(other)
                   && Equals(_session, other._session);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;

            return Equals((NhWorkEntry)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (_session != null ? _session.GetHashCode() : 0);
            }
        }

        public override IWorkEntry Save()
        {
            using (var tx = _session.BeginTransaction())
            {
                var nhWorkEntry = _session.Query<WorkEntryEntity>()
                                          .Single(p => p.Id == Id);

                nhWorkEntry.Label = Label;
                nhWorkEntry.Start = Start;
                nhWorkEntry.Duration = Duration;

                _session.Save(nhWorkEntry);

                tx.Commit();
            }

            return this;
        }

        protected override IWorkEntry CreateInstance(Guid id, IProfile profile, string label, DateTime start, TimeSpan duration)
        {
            return new NhWorkEntry(_session, id, profile, label, start, duration);
        }
    }
}
