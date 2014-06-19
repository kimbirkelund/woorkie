using System;

namespace Woorkie.Core
{
    public abstract class WorkEntryBase : IWorkEntry, IEquatable<WorkEntryBase>
    {
        private readonly TimeSpan _duration;
        private readonly Guid _id;
        private readonly string _label;
        private readonly IProfile _profile;
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

        protected WorkEntryBase(Guid id, IProfile profile, string label, DateTime start, TimeSpan duration)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");
            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentNullException("label");

            _id = id;
            _profile = profile;
            _label = label;
            _start = start;
            _duration = duration;
        }

        public bool Equals(WorkEntryBase other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Equals(_profile, other._profile)
                   && _id.Equals(other._id)
                   && string.Equals(_label, other._label)
                   && _start.Equals(other._start)
                   && _duration.Equals(other._duration);
        }

        public bool Equals(IWorkEntry other)
        {
            return Equals(other as WorkEntryBase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((WorkEntryBase)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _profile != null ? _profile.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ _id.GetHashCode();
                hashCode = (hashCode * 397) ^ (_label != null ? _label.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _start.GetHashCode();
                hashCode = (hashCode * 397) ^ _duration.GetHashCode();
                return hashCode;
            }
        }

        public abstract IWorkEntry Save();

        public IWorkEntry WithDuration(TimeSpan value)
        {
            if (value == Duration)
                return this;

            return CreateInstance(Id, Profile, Label, Start, value);
        }

        public IWorkEntry WithLabel(string value)
        {
            if (value == Label)
                return this;

            return CreateInstance(Id, Profile, value, Start, Duration);
        }

        public IWorkEntry WithStart(DateTime value)
        {
            if (value == Start)
                return this;

            return CreateInstance(Id, Profile, Label, value, Duration);
        }

        protected abstract IWorkEntry CreateInstance(Guid id, IProfile profile, string label, DateTime start, TimeSpan value);
    }
}
