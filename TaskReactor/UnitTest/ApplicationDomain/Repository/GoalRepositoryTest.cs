using System;
using System.Text.Json;
using System.Threading.Tasks;
using Data.Database.Entity;
using Data.DataRepository;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain.Repository
{
    public sealed class GoalRepositoryTest : RepositoryTest<Goal, IGoalRepository>, IDisposable
    {
        bool _disposed;

        [NotNull] readonly User _testUser = new User
        {
            Name = "first", Password = "123", Tasks = new[]
            {
                new UserTask
                {
                    Title = "Finish Homework",
                    Description = "Cooperation",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now + TimeSpan.FromDays(2)
                }
            }
        };

        [NotNull] UserTask _testTask => _testUser.Tasks![0];

        [NotNull] readonly IUserRepository _userRepository;

        [NotNull, ItemNotNull] internal static Goal[] TestEntities => new[]
        {
            new Goal
            {
                Title = "C#",
                Description = "XUnit learning",
                DurationOfOneTime = TimeSpan.FromHours(1),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Interval = new Interval { Kind = IntervalKind.ByDay, Value = 1 }
            },
            new Goal
            {
                Title = "C++",
                Description = "CPP20 features",
                DurationOfOneTime = TimeSpan.FromHours(2),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Interval = new Interval { Kind = IntervalKind.ByWeek, Value = 2 }
            },
            new Goal
            {
                Title = "WPF MVVM",
                Description = "xaml syntax learning",
                DurationOfOneTime = TimeSpan.FromHours(3),
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Interval = new Interval { Kind = IntervalKind.MonthByDay, Value = 3 }
            }
        };

        public GoalRepositoryTest([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _userRepository = Container.GetExportedValue<IUserRepository>();
            _userRepository.Register(_testUser);
            Task.WaitAll(Repository.DbSync());
        }

        async Task AddToTask([NotNull] Goal goal)
        {
            Repository.AddToTask(_testTask, goal);
            await Repository.DbSync();

            TestOutputHelper.WriteLine(
                $"Successfully add goal {JsonSerializer.Serialize(goal, SerializerOptions)} " +
                $"\nto task {JsonSerializer.Serialize(_testTask, SerializerOptions)}\n"
            );
        }

        async Task GetAllTest() =>
            TestOutputHelper.WriteLine(
                $"All goals from task {JsonSerializer.Serialize(_testTask, SerializerOptions)}:\n" +
                JsonSerializer.Serialize(await Repository.GetAllFromTaskAsync(_testTask), SerializerOptions)
            );

        [Fact]
        async Task Test()
        {
            foreach (var goal in TestEntities) await AddToTask(goal);
            await GetAllTest();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (_disposed || !disposing) return;
            _userRepository.LogOff(_testUser);
            Task.WaitAll(Repository.DbSync());

            _disposed = true;
        }

        ~GoalRepositoryTest() => Dispose(false);
    }
}