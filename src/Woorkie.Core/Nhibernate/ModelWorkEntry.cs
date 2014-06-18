using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class ModelWorkEntry : IWorkEntry, IEquatable<ModelWorkEntry>
    {
        private readonly TimeSpan _duration;
        private readonly Guid _id;
        private readonly string _label;
        private readonly IProfile _profile;
        private readonly ISession _session;
        private readonly DateTime _start;

        public TimeSpan Duration
        {
            get { return _duration; }
        }

        public Guid Id
        {
            get { return _id; }
        }

        public string Label
        {
            get { return _label; }
        }

        public IProfile Profile
        {
            get { return _profile; }
        }

        public DateTime Start
        {
            get { return _start; }
        }

        public ModelWorkEntry(ISession session, Guid id, IProfile profile, string label, DateTime start, TimeSpan duration)
        {
            if (session == null)
                throw new ArgumentNullException("session");
            if (profile == null)
                throw new ArgumentNullException("profile");
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentNullException("label");

            _session = session;
            _id = id;
            _profile = profile;
            _label = label;
            _start = start;
            _duration = duration;
        }

        public bool Equals(ModelWorkEntry other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Equals(_session, other._session)
                   && Equals(_profile, other._profile)
                   && _id.Equals(other._id)
                   && string.Equals(_label, other._label)
                   && _start.Equals(other._start)
                   && _duration.Equals(other._duration);
        }

        public bool Equals(IWorkEntry other)
        {
            return Equals(other as ModelWorkEntry);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ModelWorkEntry)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_session != null ? _session.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_profile != null ? _profile.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _id.GetHashCode();
                hashCode = (hashCode * 397) ^ (_label != null ? _label.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _start.GetHashCode();
                hashCode = (hashCode * 397) ^ _duration.GetHashCode();
                return hashCode;
            }
        }

        public IWorkEntry Save()
        {
            using (var tx = _session.BeginTransaction())
            {
                var nhWorkEntry = _session.Query<NhWorkEntry>()
                                          .Single(p => p.Id == Id);

                nhWorkEntry.Label = Label;
                nhWorkEntry.Start = Start;
                nhWorkEntry.Duration = Duration;

                _session.Save(nhWorkEntry);

                tx.Commit();
            }

            return this;
        }

        public IWorkEntry WithDuration(TimeSpan value)
        {
            if (value == Duration)
                return this;

            return new ModelWorkEntry(_session, Id, Profile, Label, Start, value);
        }

        public IWorkEntry WithLabel(string value)
        {
            if (value == Label)
                return this;

            return new ModelWorkEntry(_session, Id, Profile, value, Start, Duration);
        }

        public IWorkEntry WithStart(DateTime value)
        {
            if (value == Start)
                return this;

            return new ModelWorkEntry(_session, Id, Profile, Label, value, Duration);
        }
    }
}
