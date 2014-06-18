using FluentNHibernate.Mapping;

namespace Woorkie.Core.Nhibernate
{
    public class NhProfileMap : ClassMap<NhProfile>
    {
        public NhProfileMap()
        {
            Table("Profile");
            Id(e => e.Name);
            Map(e => e.DefaultHoursPerWeek);
        }
    }
}
