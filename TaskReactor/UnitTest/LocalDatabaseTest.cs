using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ApplicationDomain.Models.Database;
using ApplicationDomain.Models.Database.Entity;
using ApplicationDomain.Models.Database.Repository;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Presentation;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest
{
    public sealed class LocalDatabaseTest
    {
        [NotNull] private readonly ITestOutputHelper _testOutputHelper;

        [NotNull] private readonly CompositionContainer _container = GetContainer();

        [NotNull] private readonly UserRepository _userRepository;

        [NotNull]
        static CompositionContainer GetContainer() =>
            new CompositionContainer(
                new AggregateCatalog(
                    new AssemblyCatalog(typeof(IDatabaseModel).Assembly!),
                    new AssemblyCatalog(typeof(App).Assembly!)
                )
            );

        public LocalDatabaseTest([NotNull] ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _userRepository = _container.GetExportedValue<UserRepository>();
            var context = _container.GetExportedValue<TaskReactorDbContext>();

            // clear all records every time before test
            context.DeleteTableFromDbSet<User>();
        }

        [Fact]
        public void DbContextCreateTest()
        {
            using var context = new TaskReactorDbContext();
        }

        public static readonly IEnumerable<object[]> UserTestData =
            new[]
            {
                new[]
                {
                    new User
                    {
                        Name = "first",
                        Password = "123"
                    }
                },
                new[]
                {
                    new User
                    {
                        Name = "second",
                        Password = "123"
                    }
                },
                new[]
                {
                    new User
                    {
                        Name = "third",
                        Password = "123"
                    }
                }
            };

        [Theory, MemberData(nameof(UserTestData))]
        public async void UserTest([NotNull] User user)
        {
            await UserRegisterTest(user);

            await UserLoginTest(user);
        }

        private async Task UserLoginTest([NotNull] User user)
        {
            Assert.NotNull(await _userRepository.LogInAsync(user));
            _testOutputHelper.WriteLine($"Successfully login user {JsonSerializer.Serialize(user)}");
        }

        private async Task UserRegisterTest([NotNull] User user)
        {
            _userRepository.Register(user);
            await _userRepository.DbSync();
            _testOutputHelper.WriteLine($"Successfully register user {JsonSerializer.Serialize(user)}");
        }

        [Fact]
        public void UpdateTest()
        {
            var user = new User
            {
                Name = "My First User",
                Password = "...",
                Tasks = new List<UserTask>
                {
                    new UserTask
                    {
                        Title = "My First Task",
                        Goals = new List<Goal>
                        {
                            new Goal
                            {
                                Title = "My First Goal",
                                Interval = new Interval {Value = 1, Kind = IntervalKind.ByDay}
                            }
                        }
                    }
                }
            };

            _userRepository.Update(user);
            Task.WaitAll(_userRepository.DbSync());

            _testOutputHelper.WriteLine(user.Id.ToString());
        }

        [Fact]
        public void ReadTest()
        {
            var goalRepository = _container.GetExportedValue<GoalRepository>();

            var goal = goalRepository.DbSet!.FirstAsync()?.Result;
            _testOutputHelper.WriteLine(goal?.Interval.Value.ToString());
        }
    }
}