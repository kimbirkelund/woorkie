using NHibernate;

namespace Woorkie.Core.Nhibernate
{
    public static class NhEntityExtensions
    {
        public static IWorkEntry ToModelEntity(this NhWorkEntry entity, ISession session)
        {
            return new ModelWorkEntry(session,
                                      entity.Id,
                                      entity.Profile.ToModelEntity(session),
                                      entity.Label,
                                      entity.Start,
                                      entity.Duration);
        }

        public static IProfile ToModelEntity(this NhProfile entity, ISession session)
        {
            return new ModelProfile(session, entity.Name, entity.DefaultHoursPerWeek);
        }
    }
}
