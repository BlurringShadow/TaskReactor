using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Threading.Tasks;
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
    public sealed class DataBaseTest
    {
        [NotNull] private readonly ITestOutputHelper _testOutputHelper;

        [NotNull] private readonly CompositionContainer _container = new CompositionContainer(
            new AggregateCatalog(
                new AssemblyCatalog(Assembly.GetAssembly(typeof(TaskReactorDbContext))!),
                new AssemblyCatalog(Assembly.GetAssembly(typeof(ArgsHelper))!)
            )
        );

        public DataBaseTest([NotNull] ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public void DbContextCreateTest()
        {
            using var context = new TaskReactorDbContext();
        }

        [Fact]
        public void ArgsHelperExportTest()
        {
            var argsHelper = _container.GetExportedValue<ArgsHelper>();
            _testOutputHelper.WriteLine(argsHelper.ToString());
        }

        [Fact]
        public void UpdateTest()
        {
            var userRepository = _container.GetExportedValue<UserRepository>();
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

            userRepository.Update(user);
            Task.WaitAll(userRepository.DbSync());

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