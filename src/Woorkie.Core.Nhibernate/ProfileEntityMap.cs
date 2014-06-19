using FluentNHibernate.Mapping;

namespace Woorkie.Core.Nhibernate
{
    public class ProfileEntityMap : ClassMap<ProfileEntity>
    {
        public ProfileEntityMap()
        {
            Table("Profile");
            Id(e => e.Name);
            Map(e => e.DefaultHoursPerWeek);
        }
    }
}
