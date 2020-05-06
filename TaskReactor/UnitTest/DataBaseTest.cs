using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using TaskReactor.Models;
using TaskReactor.Models.DataBase;
using TaskReactor.Utilities;
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

            var testDbFilePath = Path.Combine(
                @"E:\DOCUMENTS\LEARNING\高级软件设计\TASKREACTOR\TASKREACTOR\UNITTEST\TESTINGDATABASE.MDF"
            );

            var option = new DbContextOptionsBuilder<TaskReactorDbContext>().UseSqlServer(
                @$"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={testDbFilePath};Integrated Security=True;"
            );

            _context = new TaskReactorDbContext(option!.Options!);
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

            _context.Users.Add(user);
            _context.SaveChanges();

            _testOutputHelper.WriteLine(user.Id.ToString());
        }

        [Fact]
        public void ReadTest()
        {
            var schedule = _context.Schedules.FirstAsync()?.Result;
            _testOutputHelper.WriteLine(schedule?.Interval.Value.ToString());
        }

        public void Dispose() => _context.Dispose();
    }
}