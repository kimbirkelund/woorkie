using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class NhibernateDbContext : IDbContext
    {
        private readonly ISession _session;

        public NhibernateDbContext(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _session = session;
        }

        public IWorkEntry AddWork(IProfile profile, string label, DateTime start, TimeSpan duration)
        {
            NhWorkEntry workEntry;
            using (var tx = _session.BeginTransaction())
            {
                workEntry = new NhWorkEntry
                {
                    Id = Guid.NewGuid(),
                    Profile = _session.Query<NhProfile>()
                                      .Single(p => p.Name == profile.Name),
                    Label = label,
                    Start = start,
                    Duration = duration,
                };

                _session.Save(workEntry);

                tx.Commit();
            }

            return workEntry.ToModelEntity(_session);
        }

        public IProfile CreateProfile(string name)
        {
            NhProfile profile;
            using (var tx = _session.BeginTransaction())
            {
                profile = new NhProfile
                {
                    Name = name
                };

                _session.Save(profile);

                tx.Commit();
            }

            return profile.ToModelEntity(_session);
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        public IProfile FindProfile(string name)
        {
            using (_session.BeginTransaction())
            {
                var nhProfile = _session.Query<NhProfile>()
                                        .SingleOrDefault(p => p.Name == name);

                if (nhProfile == null)
                    return null;

                return nhProfile.ToModelEntity(_session);
            }
        }

        public IQueryable<IWorkEntry> QueryWork()
        {
            return new NhWorkEntryQueryable(new NhWorkEntryQueryProvider(_session), null);
        }
    }
}
