using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class ModelProfile : ProfileBase, IEquatable<ModelProfile>
    {
        private readonly ISession _session;

        public ModelProfile(ISession session, string name, TimeSpan? defaultHoursPerWeek)
            : base(name, defaultHoursPerWeek)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _session = session;
        }

        public bool Equals(ModelProfile other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return base.Equals(other) && Equals(_session, other._session);
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
                return (base.GetHashCode() * 397) ^ (_session != null ? _session.GetHashCode() : 0);
            }
        }

        public override IProfile Save()
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

        protected override IProfile CreateInstance(string name, TimeSpan? defaultHoursPerWeek)
        {
            return new ModelProfile(_session, name, defaultHoursPerWeek);
        }
    }
}
