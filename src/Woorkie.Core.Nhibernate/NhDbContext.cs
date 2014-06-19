using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace Woorkie.Core.Nhibernate
{
    public class NhDbContext : IDbContext
    {
        private readonly ISession _session;

        public NhDbContext(ISession session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            _session = session;
        }

        public IWorkEntry AddWork(IProfile profile, string label, DateTime start, TimeSpan duration)
        {
            WorkEntryEntity workEntry;
            using (var tx = _session.BeginTransaction())
            {
                workEntry = new WorkEntryEntity
                {
                    Id = Guid.NewGuid(),
                    Profile = _session.Query<ProfileEntity>()
                                      .Single(p => p.Name == profile.Name),
                    Label = label,
                    Start = start,
                    Duration = duration,
                };

                _session.Save(workEntry);

                tx.Commit();
            }

            return workEntry.ToModelWOrkEntry(_session);
        }

        public IProfile CreateProfile(string name)
        {
            ProfileEntity profile;
            using (var tx = _session.BeginTransaction())
            {
                profile = new ProfileEntity
                {
                    Name = name
                };

                _session.Save(profile);

                tx.Commit();
            }

            return profile.ToModelProfile(_session);
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        public IProfile FindProfile(string name)
        {
            using (_session.BeginTransaction())
            {
                var nhProfile = _session.Query<ProfileEntity>()
                                        .SingleOrDefault(p => p.Name == name);

                if (nhProfile == null)
                    return null;

                return nhProfile.ToModelProfile(_session);
            }
        }

        public IQueryable<IWorkEntry> QueryWork()
        {
            return new NhWorkEntryQueryable(new NhWorkEntryQueryProvider(_session), null);
        }
    }
}
