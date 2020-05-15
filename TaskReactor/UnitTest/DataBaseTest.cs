using System;
using System.Collections.Generic;
using ApplicationDomain.Models.Database;
using ApplicationDomain.Models.Database.Entity;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest
{
    public sealed class DataBaseTest : IDisposable
    {
        [NotNull] private readonly ITestOutputHelper _testOutputHelper;
        [NotNull] private readonly TaskReactorDbContext _context;

        public DataBaseTest([NotNull] ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            _context = new TaskReactorDbContext();
        }

        [Fact]
        public void DeleteTest()
        {
            _context.DeleteTableFromDbSet<User>();
            _context.TruncateTableFromDbSet<Schedule>();
        }

        [Fact]
        public void UpdateTest()
        {
            var random = new Random();
            var schedule = new Schedule
            {
                Title = $"My First Schedule{random.Next()}",
                Interval = new Interval {Value = 1, Kind = IntervalKind.ByDay}
            };
            var user = new User
            {
                Name = "My First User",
                Password = "...",
                Schedules = new List<Schedule> {schedule}
            };

            _context.Set<User>()!.Add(user);
            _context.SaveChanges();

            _testOutputHelper.WriteLine(user.Id.ToString());
        }

        [Fact]
        public void ReadTest()
        {
            var schedule = _context.Set<Schedule>()!.FirstAsync()?.Result;
            _testOutputHelper.WriteLine(schedule?.Interval.Value.ToString());
        }

        public void Dispose() => _context.Dispose();
    }
}