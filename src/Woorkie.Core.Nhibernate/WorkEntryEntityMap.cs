using FluentNHibernate.Mapping;

namespace Woorkie.Core.Nhibernate
{
    public class WorkEntryEntityMap : ClassMap<WorkEntryEntity>
    {
        public WorkEntryEntityMap()
        {
            Table("WorkEntry");
            Id(e => e.Id);
            References(e => e.Profile);
            Map(e => e.Label);
            Map(e => e.Start);
            Map(e => e.Duration);
        }
    }
}
