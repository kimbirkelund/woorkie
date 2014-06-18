using System;
using System.Collections.Immutable;
using System.Linq;
using Xunit;

namespace Woorkie.Core.Test.Nhibernate
{
    public class NhibernateDbContextTest : NhibernateDbContextTestBase
    {
        [Fact]
        public void TestAddWork()
        {
            using (var sut = CreateContext())
            {
                var profile = sut.CreateProfile(Guid.NewGuid().ToString());
                const string label = "work";
                var start = DateTime.Today.AddHours(7);
                var duration = TimeSpan.FromHours(7.5);

                var work = sut.AddWork(profile, label, start, duration);

                Assert.NotNull(work);
                Assert.Equal(profile, work.Profile);
                Assert.Equal(label, work.Label);
                Assert.Equal(start, work.Start);
                Assert.Equal(duration, work.Duration);
            }
        }

        [Fact]
        public void TestQueryWork()
        {
            using (var sut = CreateContext())
            {
                var profile = sut.CreateProfile(Guid.NewGuid().ToString());
                const string label = "work";
                var start = DateTime.Today.AddHours(7);
                var duration = TimeSpan.FromHours(7.5);

                sut.AddWork(profile, label, start, duration);
                sut.AddWork(profile, label, start.AddDays(1), duration);
                sut.AddWork(profile, label, start.AddDays(2), duration);
                sut.AddWork(profile, label, start.AddDays(3), duration);

                var wes = sut.QueryWork()
                             .Where(e => e.Profile == profile)
                             .Where(e => e.Duration > TimeSpan.FromHours(7))
                             .Where(e => e.Start == start.AddDays(1))
                             .Where(e => e.Label == "work")
                             .ToImmutableList();

                Assert.Equal(1, wes.Count);

                var work = wes.Single();
                Assert.NotNull(work);
                Assert.Equal(profile, work.Profile);
                Assert.Equal(label, work.Label);
                Assert.Equal(start.AddDays(1), work.Start);
                Assert.Equal(duration, work.Duration);
            }
        }
    }
}
