using System;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationDomain.Database;
using ApplicationDomain.Database.Entity;
using ApplicationDomain.DataRepository;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ApplicationDomain.Repository
{
    public sealed class TaskDependenciesRepositoryTest :
        RepositoryTest<TaskDependency, ITaskDependencyRepository>,
        IDisposable
    {
        [NotNull] readonly User _testUser = new User {Name = "first", Password = "123"};

        bool _disposed;

        [NotNull] readonly IUserRepository _userRepository;

        public TaskDependenciesRepositoryTest([NotNull] ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _userRepository = Container.GetExportedValue<IUserRepository>();

            _testUser.Tasks = new[]
            {
                new UserTask
                {
                    OwnerUser = _testUser,
                    Title = "Finish Homework",
                    Description = "Cooperation",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now + TimeSpan.FromDays(2)
                },
                new UserTask
                {
                    OwnerUser = _testUser,
                    Title = "Touch fish",
                    StartTime = DateTime.Now,
                    Description = "Template meta programming",
                    EndTime = DateTime.Now + TimeSpan.FromDays(3)
                },
                new UserTask
                {
                    OwnerUser = _testUser,
                    Title = "Publish new projects ",
                    StartTime = DateTime.Now,
                    Description = "Dump them halfway",
                    EndTime = DateTime.Now + TimeSpan.FromDays(4)
                }
            };

            lock(Repository.Context)
            {
                TestOutputHelper.WriteLine(
                    $"Registering user {JsonSerializer.Serialize(_testUser, SerializerOptions)}"
                );
                _userRepository.Register(_testUser);
                Task.WaitAll(Repository.DbSync());
            }

            _testUser = _userRepository.LogInAsync(_testUser).Result!;
            TestOutputHelper.WriteLine(
                $"Successfully log in user {JsonSerializer.Serialize(_testUser, SerializerOptions)}"
            );
        }

        async Task AddDependenciesTest(int targetIndex, int dependencyIndex) =>
            await Task.Run(
                () =>
                {
                    lock(Repository.Context)
                    {
                        var dependencies =
                            Repository.AddDependencies(_testUser.Tasks![targetIndex], _testUser.Tasks[dependencyIndex]);
                        Task.WaitAll(Repository.DbSync());
                        TestOutputHelper.WriteLine(
                            $"Successfully add task dependency {JsonSerializer.Serialize(dependencies, SerializerOptions)}"
                        );
                    }
                }
            );

        async Task GetDependenciesTest([NotNull] UserTask task) =>
            TestOutputHelper.WriteLine(
                $"\nTask {JsonSerializer.Serialize(task, SerializerOptions)}\nhas follow dependencies:\n" +
                JsonSerializer.Serialize(await Repository.GetDependenciesAsync(task), SerializerOptions)
            );

        [Fact]
        async Task Test()
        {
            await AddDependenciesTest(1, 0);
            /*
            await AddDependenciesTest(2, 1);
            await AddDependenciesTest(0, 2);

            foreach (var task in _testUser.Tasks!)
                await GetDependenciesTest(task);
            */
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if(_disposed || !disposing) return;
            lock(Repository.Context)
            {
                TestOutputHelper.WriteLine(
                    $"Removing user {JsonSerializer.Serialize(_testUser, SerializerOptions)}"
                );
                _userRepository.LogOff(_testUser);
                Task.WaitAll(_userRepository.DbSync());
                TestOutputHelper.WriteLine("Successfully removed");
            }

            _disposed = true;
        }

        ~TaskDependenciesRepositoryTest() => Dispose(false);
    }
}