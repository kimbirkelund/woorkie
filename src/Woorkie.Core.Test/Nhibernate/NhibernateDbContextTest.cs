using System;
using System.Collections.Immutable;
using System.Linq;
using Woorkie.Core.Nhibernate;
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
        public void TestConstructor_InvalidArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new NhibernateDbContext(null));
        }

        [Fact]
        public void TestFindProfile_DoesntExist()
        {
            using (var sut = CreateContext())
                Assert.Null(sut.FindProfile(Guid.NewGuid().ToString()));
        }

        [Fact]
        public void TestFindProfile_Exists()
        {
            using (var sut = CreateContext())
            {
                var name = Guid.NewGuid().ToString();

                sut.CreateProfile(name);

                var profile = sut.FindProfile(name);
                Assert.NotNull(profile);
                Assert.Equal(name, profile.Name);
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
                             .Where(e => e.Profile.Equals(profile))
                             .Where(e => e.Duration > TimeSpan.FromHours(7))
                             .Where(e => start.AddHours(20) <= e.Start && e.Start <= start.AddHours(40))
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

        [Fact]
        public void TestUpdateProfile_ValidatedByQuery()
        {
            using (var sut = CreateContext())
            {
                var profile = sut.CreateProfile(Guid.NewGuid().ToString());

                Assert.Null(profile.DefaultHoursPerWeek);

                var defaultHoursPerWeek = TimeSpan.FromHours(37.5);

                profile = profile.WithDefaultHoursPerWeek(defaultHoursPerWeek)
                                 .Save();

                Assert.Equal(defaultHoursPerWeek, profile.DefaultHoursPerWeek);

                profile = sut.FindProfile(profile.Name);
                Assert.NotNull(profile);
                Assert.Equal(defaultHoursPerWeek, profile.DefaultHoursPerWeek);
            }
        }

        [Fact]
        public void TestUpdateWorkEntry_ValidatedByQuery()
        {
            using (var sut = CreateContext())
            {
                var profile = sut.CreateProfile(Guid.NewGuid().ToString());
                const string label = "work";
                var start = DateTime.Today.AddHours(7);
                var duration = TimeSpan.FromHours(7.5);

                var work = sut.AddWork(profile, label, start, duration);

                const string newLabel = "holiday";
                var newStart = DateTime.Today;
                var newDuration = TimeSpan.FromHours(7.5 / 2);

                work = work.WithLabel(newLabel)
                           .WithStart(newStart)
                           .WithDuration(newDuration)
                           .Save();

                Assert.Equal(profile, work.Profile);
                Assert.Equal(newLabel, work.Label);
                Assert.Equal(newStart, work.Start);
                Assert.Equal(newDuration, work.Duration);

                work = sut.QueryWork().Single(e => e.Id == work.Id);

                Assert.Equal(profile, work.Profile);
                Assert.Equal(newLabel, work.Label);
                Assert.Equal(newStart, work.Start);
                Assert.Equal(newDuration, work.Duration);
            }
        }
    }
}
