using System;

namespace Woorkie.Core
{
    public abstract class ProfileBase : IProfile, IEquatable<ProfileBase>
    {
        private readonly TimeSpan? _defaultHoursPerWeek;
        private readonly string _name;

        public TimeSpan? DefaultHoursPerWeek
        {
            get { return _defaultHoursPerWeek; }
        }

        public string Name
        {
            get { return _name; }
        }

        protected ProfileBase(string name, TimeSpan? defaultHoursPerWeek)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            _name = name;
            _defaultHoursPerWeek = defaultHoursPerWeek;
        }

        public bool Equals(ProfileBase other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return string.Equals(_name, other._name)
                   && _defaultHoursPerWeek.Equals(other._defaultHoursPerWeek);
        }

        public bool Equals(IProfile other)
        {
            return Equals(other as ProfileBase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ProfileBase)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _name != null ? _name.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ _defaultHoursPerWeek.GetHashCode();
                return hashCode;
            }
        }

        public abstract IProfile Save();

        public IProfile WithDefaultHoursPerWeek(TimeSpan? value)
        {
            if (value == DefaultHoursPerWeek)
                return this;

            return CreateInstance(Name, value);
        }

        protected abstract IProfile CreateInstance(string name, TimeSpan? defaultHoursPerWeek);
    }
}
