using System;
using NHibernate;

namespace Woorkie.Core.Nhibernate
{
    public static class WorkEntryEntityExtensions
    {
        public static IWorkEntry ToModelWOrkEntry(this WorkEntryEntity entity, ISession session)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (session == null)
                throw new ArgumentNullException("session");

            return new NhWorkEntry(session,
                                   entity.Id,
                                   entity.Profile.ToModelProfile(session),
                                   entity.Label,
                                   entity.Start,
                                   entity.Duration);
        }
    }
}
