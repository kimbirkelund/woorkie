using FluentNHibernate.Mapping;

namespace Woorkie.Core.Nhibernate
{
    public class NhWorkEntryMap : ClassMap<NhWorkEntry>
    {
        public NhWorkEntryMap()
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
