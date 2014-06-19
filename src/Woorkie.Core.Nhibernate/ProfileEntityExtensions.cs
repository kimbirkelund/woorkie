using System;
using NHibernate;

namespace Woorkie.Core.Nhibernate
{
    public static class ProfileEntityExtensions
    {
        public static IProfile ToModelProfile(this ProfileEntity entity, ISession session)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (session == null)
                throw new ArgumentNullException("session");

            return new NhProfile(session, entity.Name, entity.DefaultHoursPerWeek);
        }
    }
}
