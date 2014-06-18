using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class ModelProfile : IProfile, IEquatable<ModelProfile>
    {
        private readonly TimeSpan? _defaultHoursPerWeek;
        private readonly string _name;
        private readonly ISession _session;

        public TimeSpan? DefaultHoursPerWeek
        {
            get { return _defaultHoursPerWeek; }
        }

        public string Name
        {
            get { return _name; }
        }

        public ModelProfile(ISession session, string name, TimeSpan? defaultHoursPerWeek)
        {
            if (session == null)
                throw new ArgumentNullException("session");
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            _session = session;
            _name = name;
            _defaultHoursPerWeek = defaultHoursPerWeek;
        }

        public bool Equals(ModelProfile other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Equals(_session, other._session)
                   && string.Equals(_name, other._name)
                   && _defaultHoursPerWeek.Equals(other._defaultHoursPerWeek);
        }

        public bool Equals(IProfile other)
        {
            return Equals(other as ModelProfile);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((ModelProfile)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_session != null ? _session.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _defaultHoursPerWeek.GetHashCode();
                return hashCode;
            }
        }

        public IProfile Save()
        {
            using (var tx = _session.BeginTransaction())
            {
                var nhProfile = _session.Query<NhProfile>()
                                        .Single(p => p.Name == Name);

                nhProfile.DefaultHoursPerWeek = DefaultHoursPerWeek;

                _session.Save(nhProfile);

                tx.Commit();
            }

            return this;
        }

        public IProfile WithDefaultHoursPerWeek(TimeSpan? value)
        {
            if (value == DefaultHoursPerWeek)
                return this;

            return new ModelProfile(_session, Name, value);
        }
    }
}
